namespace UniStore.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Data.UnitOfWork;
    using Interfaces;
    using Models;
    using Models.BindingModels.Manufacturer;
    using Models.EntityModels;
    using Models.ViewModels.Manufacturer;

    public class ManufacturersService : BaseService, IManufacturersService
    {
        public ManufacturersService(IUniStoreContext context) : base(context)
        {
        }

        public IEnumerable<Manufacturer> FindManufacturers(string search)
        {
            return this.Context.Manufacturers.All().Where(m => m.Name.Contains(search));
        }

        public Manufacturer GetManufacturerById(int id)
        {
            return this.Context.Manufacturers.Find(id);
        }

        public void AddManufacturer(AddManufacturerBM manufacturerBM)
        {
            var manufacturer = Mapper.Map<Manufacturer>(manufacturerBM);
            this.Context.Manufacturers.Add(manufacturer);
            this.Context.SaveChanges();
        }

        public void UpdateManufacturer(EditManufacturerBM manufacturerBM)
        {
            var manufacturer = this.Context.Manufacturers.Find(manufacturerBM.Id);
            if (manufacturer == null)
            {
                return;
            }

            manufacturer.Name = manufacturerBM.Name;
            manufacturer.ContactInformation = manufacturerBM.ContactInformation;
            this.Context.SaveChanges();
        }

        public bool IsExistManufacturer(int id)
        {
            return this.Context.Manufacturers.Find(id) != null;
        }

        public void RemoveManufacturer(int id)
        {
            this.Context.Manufacturers.Remove(id);
            this.Context.SaveChanges();
        }

        public ManufacturersListVM GetManufacturerListVM(string search, int? page)
        {
            const int PageSize = 2;

            var pageNumber = page ?? 1;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var manufacturers = this.Context.Manufacturers.All()
                .Where(l => search == null || l.Name.ToLower().Contains(search.ToLower()))
                .ToList()
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(Mapper.Map<ManufacturerVM>)
                .ToArray();
            var manufacturersCount = this.Context.Manufacturers.All()
                .Count(l => search == null || l.Name.ToLower().Contains(search.ToLower()));
            var pageCount = manufacturersCount / PageSize + (manufacturersCount % PageSize > 0 ? 1 : 0);
            var manufacturersListVM = new ManufacturersListVM
            {
                Manufacturers = manufacturers,
                Pagination = new Pagination
                {
                    Page = pageNumber,
                    PageCount = pageCount,
                    Search = search
                }
            };

            return manufacturersListVM;
        }
    }
}