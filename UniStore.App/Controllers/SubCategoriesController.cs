namespace UniStore.App.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using Attributes;
    using AutoMapper;
    using Data.UnitOfWork;
    using Models.BindingModels.SubCategory;
    using Models.EntityModels;
    using Models.Enums;
    using Models.ViewModels.SubCategory;
    using Services.Interfaces;

    [AuthorizeInRole(AppRole.Administrator)]
    [RoutePrefix("subcategories")]
    public class SubCategoriesController : BaseController
    {
        private readonly ISubCategoriesService service;

        public SubCategoriesController(IUniStoreContext context, ISubCategoriesService service)
            : base(context)
        {
            this.service = service;
        }

        public SubCategoriesController(IUniStoreContext context, ISubCategoriesService service, User user)
            : base(context, user)
        {
            this.service = service;
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}")]
        public ActionResult SubCategoriesList(int categoryId)
        {
            var categoryVM = this.service.GetCategorySubCategoriesVM(categoryId);
            return this.PartialView("Partials/SubCategoriesList", categoryVM);
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{id:regex(\d+)}")]
        public ActionResult Details(int departmentId, int categoryId, int id)
        {
            var subCategoryVM = this.service.GetDetailsSubCategoryVM(departmentId, categoryId, id);
            if (subCategoryVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("Partials/Details",subCategoryVM);
            }

            return this.PartialView(subCategoryVM);
        }

        [HttpGet]
        [Route(@"category/{categoryId:regex(\d+)}/panel")]
        [AllowAnonymous]
        public ActionResult SubCategoriesPanel(int categoryId)
        {
            var subCategoriesVM = this.service.GetCategorySubCategoriesVM(categoryId);
            if (subCategoriesVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return this.PartialView("Partials/SubCategoriesPanel", subCategoriesVM);
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/add")]
        public ActionResult Add(int departmentId, int categoryId)
        {
            return this.PartialView(
                "Partials/Add",
                new AddSubCategoryVM { DepartmentId = departmentId, CategoryId = categoryId });
        }

        [HttpPost]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/add")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int departmentId, int categoryId, AddSubCategoryBM subCategoryBM)
        {
            if (this.service.IsExistSubCategoryWithName(departmentId, categoryId, subCategoryBM.Name))
            {
                this.ModelState.AddModelError(
                    nameof(subCategoryBM.Name),
                    $"The subcategory with name {subCategoryBM.Name} already exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.PartialView("Partials/Add", Mapper.Map<AddSubCategoryVM>(subCategoryBM));
            }

            if (!this.service.AddSubCategory(departmentId, categoryId, subCategoryBM))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction("SubCategoriesList", new { departmentId, categoryId });
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{id:regex(\d+)}/rename")]
        public ActionResult Rename(int departmentId, int categoryId, int id)
        {
            var categoryVM = this.service.GetRenameSubCategoryVM(departmentId, categoryId, id);
            if (categoryVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.PartialView("Partials/Rename", categoryVM);
        }

        [HttpPost]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{id:regex(\d+)}/rename")]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(int departmentId, int categoryId, int id, RenameSubCategoryBM subCategoryBM)
        {
            if (this.service.IsExistOtherCategoryWithName(departmentId, categoryId, id, subCategoryBM.Name))
            {
                this.ModelState.AddModelError(
                    nameof(subCategoryBM.Name),
                    $"The category with name {subCategoryBM.Name} already exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.PartialView("Partials/Rename", Mapper.Map<RenameSubCategoryVM>(subCategoryBM));
            }

            if (!this.service.RenameCategory(departmentId, categoryId, id, subCategoryBM.Name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction("SubCategoriesList", new { departmentId, categoryId });
        }

        [HttpPost]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{id:regex(\d+)}/delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int departmentId, int categoryId, int id)
        {
            if (!this.service.RemoveSubCategory(departmentId, categoryId, id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction("SubCategoriesList", new { departmentId, categoryId });
        }
    }
}