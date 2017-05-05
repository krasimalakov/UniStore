namespace UniStore.App.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Attributes;
    using AutoMapper;
    using Data.UnitOfWork;
    using Models.BindingModels.Departments;
    using Models.EntityModels;
    using Models.Enums;
    using Models.ViewModels.Department;
    using Services.Interfaces;

    [AuthorizeInRole(AppRole.Administrator)]
    [RoutePrefix("departments")]
    public class DepartmentsController : BaseController
    {
        private readonly IDepartmentsService service;

        public DepartmentsController(IUniStoreContext context, IDepartmentsService service)
            : base(context)
        {
            this.service = service;
        }

        public DepartmentsController(IUniStoreContext context, IDepartmentsService service, User user)
            : base(context, user)
        {
            this.service = service;
        }

        [HttpGet]
        [Route]
        public ActionResult All()
        {
            var departments = this.service.GetDepartmentVMs().ToList();
            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView(departments);
            }

            return this.View(departments);
        }

        [HttpGet]
        [Route("list")]
        public ActionResult DepartmentsList()
        {
            var departments = this.service.GetDepartmentVMs();
            return this.PartialView("Partials/DepartmentsList", departments);
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}")]
        public ActionResult Details(int departmentId)
        {
            var departmentVM = this.service.GetDepartmentVM(departmentId);
            if (departmentVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView(departmentVM);
            }

            return this.View(departmentVM);
        }

        [HttpGet]
        [Route("add")]
        public ActionResult Add()
        {
            return this.PartialView("Partials/Add");
        }


        [HttpPost]
        [Route("add")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddDepartmentBM departmentBM)
        {
            if (this.service.IsExistDepartmentWithName(departmentBM.Name))
            {
                this.ModelState.AddModelError(
                    nameof(departmentBM.Name),
                    $"Department with name {departmentBM.Name} already exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.PartialView("Partials/Add", departmentBM);
            }

            this.service.CreateDepartment(departmentBM);

            return this.RedirectToAction("DepartmentsList");
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/rename")]
        public ActionResult Rename(int departmentId)
        {
            var department = this.service.GetDepartmentById(departmentId);
            if (department == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return this.PartialView("Partials/Rename", Mapper.Map<DetailsDepartmentVM>(department));
        }

        [HttpPost]
        [Route(@"{departmentId:regex(\d+)}/rename")]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(int departmentId, EditDepartmentBM departmentBM)
        {
            if (this.service.IsExistOtherDepartmentWithName(departmentBM.Id, departmentBM.Name))
            {
                this.ModelState.AddModelError(
                    nameof(departmentBM.Name),
                    $"Department with name {departmentBM.Name} already exist!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.PartialView("Partials/Rename", Mapper.Map<DepartmentVM>(departmentBM));
            }

            this.service.UpdateDepartment(departmentBM);

            return this.RedirectToAction("DepartmentsList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(@"{departmentId:regex(\d+)}/delete")]
        public ActionResult Delete(int departmentId)
        {
            this.service.RemoveDepartment(departmentId);

            return this.RedirectToAction("DepartmentsList");
        }
    }
}