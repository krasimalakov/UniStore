namespace UniStore.App.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Data.UnitOfWork;
    using Models;
    using Models.EntityModels;
    using Models.Enums;
    using Models.ViewModels.Order;
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


            return this.PartialView("Partials/OrdersList", ordersListVM);
        }

        [HttpGet]
        [Authorize]
        [Route(@"{orderId:regex(\d+)}")]
        public ActionResult Details(int orderId, Pagination pagination)
        {
            var orderVM = this.service.GetOrderVM(orderId);
            if (orderVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!this.User.IsInRole(Enum.GetName(typeof(AppRole), AppRole.Administrator)))
            {
                var username = this.User.Identity.Name;
                if (!string.Equals(username, orderVM.User.UserName))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }

            var detailOrderVM=new DetailsOrderVM
            {
                Order = orderVM,
                Pagination = pagination
            };

            return this.PartialView("Partials/Details", detailOrderVM);
        }

        [HttpPost]
        [Authorize]
        [Route(@"{orderId:regex(\d+)}/update")]
        public ActionResult UpdateOrder(int orderId, Pagination pagination)
        {
            return null;
        }
    }
}