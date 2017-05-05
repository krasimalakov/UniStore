namespace UniStore.Services.Interfaces
{
    using System.Collections.Generic;
    using Models.BindingModels;
    using Models.BindingModels.Manufacturer;
    using Models.EntityModels;
    using Models.ViewModels.Manufacturer;

    public interface IManufacturersService
    {
        IEnumerable<Manufacturer> FindManufacturers(string search);

        Manufacturer GetManufacturerById(int id);

        void AddManufacturer(AddManufacturerBM manufacturerBM);

        void UpdateManufacturer(EditManufacturerBM manufacturerBM);

        bool IsExistManufacturer(int id);

        void RemoveManufacturer(int id);

        ManufacturersListVM GetManufacturerListVM(string search, int? page);
    }
}