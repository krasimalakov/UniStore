namespace UniStore.App.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Attributes;
    using Data.UnitOfWork;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.BindingModels.Order;
    using Models.BindingModels.Product;
    using Models.EntityModels;
    using Models.Enums;
    using Services.Interfaces;

    [RoutePrefix("store")]
    public class StoreController : BaseController
    {
        private readonly IStoreService service;

        public StoreController(IUniStoreContext context, IStoreService service)
            : base(context)
        {
            this.service = service;
        }

        public StoreController(IUniStoreContext context, IStoreService service, User user)
            : base(context, user)
        {
            this.service = service;
        }

        [HttpGet]
        [Route]
        public ActionResult Store()
        {
            return this.View();
        }
        
        [HttpGet]
        [Route("products")]
        public ActionResult ProductsList(SearchProductsBM searchProductsBM)
        {
            var userId = this.User.Identity.GetUserId();
            var productsListVM = this.service.GetProductsListVM(searchProductsBM, userId);
            if (productsListVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return this.PartialView("Partials/ProductsList", productsListVM);
        }

        [HttpGet]
        [AuthorizeInRole(AppRole.User)]
        [Route("shopping-cart")]
        public ActionResult ShoppingCart(SearchProductsBM searchProductsBM)
        {
            var shoppingCardVM = this.service.GetUserShoppingCardVM(
                this.User.Identity.GetUserId(),
                searchProductsBM);

            if (shoppingCardVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return this.PartialView("Partials/ShoppingCart", shoppingCardVM);
        }

        [HttpPost]
        [AuthorizeInRole(AppRole.User)]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductToShoppingCart(SearchProductsBM searchProductsBM, int productId)
        {
            if (!this.service.AddProductToUserShoppingCard(this.User.Identity.GetUserId(), productId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable);
            }

            return this.RedirectToAction("ProductsList", searchProductsBM);
        }

        [HttpPost]
        [AuthorizeInRole(AppRole.User)]
        [Route("shopping-cart/update")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateShoppingCart(SearchProductsBM searchProductsBM, int purchaseId, int quantity)
        {
            if (!this.service.UpdatePurchaseToUserShoppingCard(this.User.Identity.GetUserId(), purchaseId, quantity))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction("ShoppingCart", searchProductsBM);
        }

        [HttpGet]
        [AuthorizeInRole(AppRole.User)]
        [Route("shopping-cart/finish")]
        public ActionResult FinishOrder(SearchProductsBM searchProductsBM)
        {
            var finishOrderVM = this.service.GetUserFinishOrderVM(this.User.Identity.GetUserId(), searchProductsBM);
            if (finishOrderVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.PartialView("Partials/FinishOrder", finishOrderVM);
        }

        [HttpPost]
        [AuthorizeInRole(AppRole.User)]
        [Route("shopping-cart/finish")]
        public ActionResult FinishOrder(FinishOrderBM finishOrderBM)
        {
            var userId = this.User.Identity.GetUserId();
            var finishOrderVM = this.service.GetUserFinishOrderVM(userId, finishOrderBM.SearchProductsBM);

            if (finishOrderVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            finishOrderVM.DeliveryAddress = finishOrderBM.DeliveryAddress;

            if (!this.ModelState.IsValid)
            {
                return this.PartialView("Partials/FinishOrder", finishOrderVM);
            }

            if (!this.service.FinishOrder(userId, finishOrderVM))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction("ProductsList", finishOrderBM.SearchProductsBM);
        }

    }
}