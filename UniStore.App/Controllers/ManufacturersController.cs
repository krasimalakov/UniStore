namespace UniStore.App.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using AutoMapper;
    using Data.UnitOfWork;
    using Models.BindingModels.Manufacturer;
    using Models.EntityModels;
    using Models.Enums;
    using Models.ViewModels.Manufacturer;
    using Services.Interfaces;

    [AuthorizeInRole(AppRole.Administrator)]
    [RoutePrefix("manufacturers")]
    public class ManufacturersController : BaseController
    {
        private readonly IManufacturersService service;

        public ManufacturersController(IUniStoreContext context, IManufacturersService service)
            : base(context)
        {
            this.service = service;
        }

        public ManufacturersController(IUniStoreContext context, IManufacturersService service, User user)
            : base(context, user)
        {
            this.service = service;
        }

        [HttpGet]
        [Route]
        public ActionResult All()
        {
            return this.View();
        }

        [HttpGet]
        [Route("ManufacturersList")]
        public ActionResult ManufacturersList(string search, int? page)
        {
            var manufacturersListVM = this.service.GetManufacturerListVM(search, page);
            return this.PartialView("_ListManufacturers", manufacturersListVM);
        }

        [HttpGet]
        [Route(@"{id:regex(\d+)}")]
        public ActionResult Details(int id)
        {
            var manufacturer = this.service.GetManufacturerById(id);
            if (manufacturer == null)
            {
                return this.HttpNotFound();
            }

            return this.View(manufacturer);
        }

        [HttpGet]
        [Route("add")]
        public ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Route("add")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddManufacturerBM manufacturerBM)
        {
            if (this.ModelState.IsValid)
            {
                this.service.AddManufacturer(manufacturerBM);
                return this.RedirectToAction("All");
            }

            return this.View(manufacturerBM);
        }

        [HttpGet]
        [Route(@"{id:regex(\d+)}/edit")]
        public ActionResult Edit(int id)
        {
            var manufacturer = this.service.GetManufacturerById(id);
            if (manufacturer == null)
            {
                return this.HttpNotFound();
            }

            var manufacturerVM = Mapper.Map<ManufacturerVM>(manufacturer);
            return this.View(manufacturerVM);
        }

        [HttpPost]
        [Route(@"{id:regex(\d+)}/edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditManufacturerBM manufacturerBM)
        {
            if (this.ModelState.IsValid)
            {
                if (!this.service.IsExistManufacturer(manufacturerBM.Id))
                {
                    return this.HttpNotFound();
                }

                this.service.UpdateManufacturer(manufacturerBM);
                return this.RedirectToAction("All");
            }

            return this.View(Mapper.Map<ManufacturerVM>(manufacturerBM));
        }

        [HttpGet]
        [Route(@"{id:regex(\d+)}/delete")]
        public ActionResult Delete(int id)
        {
            var manufacturer = this.service.GetManufacturerById(id);
            if (manufacturer == null)
            {
                return this.HttpNotFound();
            }

            var manufacturerVM = Mapper.Map<ManufacturerVM>(manufacturer);

            return this.View(manufacturerVM);
        }

        [HttpPost]
        [Route(@"{id:regex(\d+)}/delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!this.service.IsExistManufacturer(id))
            {
                return this.HttpNotFound();
            }

            this.service.RemoveManufacturer(id);

            return this.RedirectToAction("All");
        }
    }
}