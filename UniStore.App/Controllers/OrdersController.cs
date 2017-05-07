namespace UniStore.App.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Data.UnitOfWork;
    using Models;
    using Models.EntityModels;
    using Models.Enums;
    using Services.Interfaces;

    [RoutePrefix("orders")]
    public class OrdersController : BaseController
    {
        private readonly IOrdersService service;

        public OrdersController(IUniStoreContext context, IOrdersService service)
            : base(context)
        {
            this.service = service;
        }

        public OrdersController(IUniStoreContext context, IOrdersService service, User user)
            : base(context, user)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize]
        [Route("{username}")]
        [Route]
        public ActionResult Orders(string username)
        {
            if (string.IsNullOrEmpty(username) &&
                !this.User.IsInRole(Enum.GetName(typeof(AppRole), AppRole.Administrator)))
            {
                username = this.User.Identity.Name;
            }

            var pagination = new Pagination { Search = username, Page = 1 };

            return this.View(pagination);
        }

        [HttpGet]
        [Authorize]
        [Route("orders")]
        public ActionResult OrdersList(Pagination pagination)
        {
            var currentUsername = this.User.Identity.Name;
            var username = pagination.Search;

            if (string.IsNullOrEmpty(username)&&
                !this.User.IsInRole(Enum.GetName(typeof(AppRole), AppRole.Administrator)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!this.User.IsInRole(Enum.GetName(typeof(AppRole), AppRole.Administrator)))
            {
                if (!string.Equals(currentUsername, username))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

                if (!this.service.IsExistUser(username))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

            }




            pagination.Search = username;

            var ordersListVM = this.service.GetOrdersVM(pagination);


            return this.PartialView(ordersListVM);
        }
    }
}