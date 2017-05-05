namespace UniStore.App.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using Attributes;
    using Data.UnitOfWork;
    using Models.BindingModels.Category;
    using Models.BindingModels.Departments;
    using Models.EntityModels;
    using Models.Enums;
    using Models.ViewModels.Category;
    using Services.Interfaces;

    [AuthorizeInRole(AppRole.Administrator)]
    [RoutePrefix("categories")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService service;

        public CategoriesController(IUniStoreContext context, ICategoriesService service)
            : base(context)
        {
            this.service = service;
        }

        public CategoriesController(IUniStoreContext context, ICategoriesService service, User user)
            : base(context, user)
        {
            this.service = service;
        }
        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}")]
        public ActionResult Details(int departmentId, int categoryId)
        {
            var categoryVM = this.service.GetDetailsCategoryVM(departmentId, categoryId);
            if (categoryVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView(categoryVM);
            }

            return this.View(categoryVM);
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}")]
        public ActionResult DepartmentCategoriesList(int departmentId)
        {
            var categoryVMs = this.service.GetDepartmentCategoriesVM(departmentId);
            return this.PartialView("Partials/CategoriesList", categoryVMs);
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/add")]
        public ActionResult Add(int departmentId)
        {
            return this.PartialView("Partials/Add", new AddCategoryVM { DepartmentId = departmentId });
        }

        [HttpPost]
        [Route(@"{departmentId:regex(\d+)}/add")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int departmentId, CategoryBM categoryBM)
        {
            if (this.service.IsExistCategoryWithName(departmentId, categoryBM.Name))
            {
                this.ModelState.AddModelError(
                    nameof(categoryBM.Name),
                    $"The category with name {categoryBM.Name} already exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.PartialView(
                    "Partials/Add",
                    new AddCategoryVM
                    {
                        DepartmentId = departmentId,
                        Name = categoryBM.Name
                    });
            }

            if (!this.service.AddCategory(departmentId, categoryBM))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction("DepartmentCategoriesList", departmentId);
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{id:regex(\d+)}/rename")]
        public ActionResult Rename(int departmentId, int id)
        {
            var categoryVM = this.service.GetRenameCategoryVM(departmentId, id);
            if (categoryVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.PartialView("Partials/Rename", categoryVM);
        }

        [HttpPost]
        [Route(@"{departmentId:regex(\d+)}/{id:regex(\d+)}/rename")]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(int departmentId, int id, CategoryBM categoryBM)
        {
            if (this.service.IsExistOtherCategoryWithName(departmentId, id, categoryBM.Name))
            {
                this.ModelState.AddModelError(
                    nameof(categoryBM.Name),
                    $"The category with name {categoryBM.Name} already exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.PartialView(
                    "Partials/Rename",
                    new RenameCategoryVM
                    {
                        DepartmentId = departmentId,
                        Id = id,
                        Name = categoryBM.Name
                    });
            }

            if (!this.service.RenameCategory(departmentId, id, categoryBM.Name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            return this.RedirectToAction("DepartmentCategoriesList", departmentId);
        }

        [HttpPost]
        [Route(@"{departmentId:regex(\d+)}/{id:regex(\d+)}/delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int departmentId, int id)
        {
            if (!this.service.RemoveCategory(departmentId, id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction("DepartmentCategoriesList", departmentId);
        }
    }
}