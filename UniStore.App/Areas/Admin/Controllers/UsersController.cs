namespace UniStore.App.Areas.Admin.Controllers
{
    using System;
    using System.Web.Mvc;
    using App.Controllers;
    using Attributes;
    using AutoMapper;
    using Data.UnitOfWork;
    using Models.BindingModels;
    using Models.BindingModels.User;
    using Models.EntityModels;
    using Models.Enums;
    using Models.ViewModels.User;
    using Services;
    using Services.Interfaces;

    [RouteArea("Admin", AreaPrefix = "admin")]
    [RoutePrefix("users")]
    public class UsersController : BaseController
    {
        private readonly IAdminService service;

        public UsersController(IUniStoreContext context, IAdminService service) 
            : base(context)
        {
            this.service = service;
        }

        public UsersController(IUniStoreContext context, IAdminService service, User user) 
            : base(context, user)
        {
            this.service = service;
        }

        [HttpGet]
        [Route]
        [AuthorizeInRole(AppRole.Administrator)]
        public ActionResult All()
        {
            var userVms = this.service.GetAllUserVMs();

            return this.View(userVms);
        }

        [HttpGet]
        [Route("{username}")]
        [AuthorizeInRole(AppRole.Administrator)]
        public ActionResult Details(string username)
        {
            if (username == null)
            {
                return this.RedirectToAction("All");
            }

            var userVM = this.service.GetUserFullVM(username);
            if (userVM == null)
            {
                return this.HttpNotFound();
            }

            return this.View(userVM);
        }


        [HttpGet]
        [Route("{username}/edit")]
        [AuthorizeInRole(AppRole.Administrator, AppRole.User, AppRole.Accountant, AppRole.Sealer)]
        public ActionResult Edit(string username)
        {
            if (username == null)
            {
                return this.RedirectToAction("All");
            }

            var userVM = this.service.GetEditUserVM(username);
            if (userVM == null)
            {
                return this.HttpNotFound();
            }

            return this.View(userVM);
        }

        [HttpPost]
        [Route("{username}/edit")]
        [AuthorizeInRole(AppRole.Administrator, AppRole.User, AppRole.Accountant, AppRole.Sealer)]
        public ActionResult Edit(EditUserBM userBM)
        {
            if (!this.ModelState.IsValid)
            {
                var userVM = Mapper.Map<EditUserVM>(userBM);
                return this.View(userVM);
            }

            var isAdmin = this.User.IsInRole(Enum.GetName(typeof(AppRole), AppRole.Administrator));
            this.service.UpdateUserData(userBM, isAdmin);

            if (isAdmin)
            {
                return this.RedirectToAction("All");
            }

            return this.RedirectToAction("Index","Store", new { area = "" });
        }

        [HttpGet]
        [Route("{username}/delete")]
        [AuthorizeInRole(AppRole.Administrator)]
        public ActionResult Delete(string username)
        {
            if (username == null || username.Equals(this.UserProfile.UserName))
            {
                return this.RedirectToAction("All");
            }

            var userVM = this.service.GetUserFullVM(username);
            if (userVM == null)
            {
                return this.HttpNotFound();
            }

            return this.View(userVM);
        }

        [HttpPost]
        [Route("{username}/delete")]
        [AuthorizeInRole(AppRole.Administrator)]
        public ActionResult Remove(string username)
        {
            this.service.RemoveUser(username, this.UserProfile.UserName);

            return this.RedirectToAction("All");
        }
    }
}