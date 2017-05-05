namespace UniStore.App.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Attributes;
    using Data.UnitOfWork;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
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

        public ActionResult Index()
        {
            if (this.Request.IsAuthenticated &&
                this.User.IsInRole(Enum.GetName(typeof(AppRole), AppRole.Administrator)))
            {
                return this.RedirectToAction("All", "Departments");
            }

            return this.RedirectToAction("Products");
        }

        [Route("products")]
        public ActionResult Products()
        {
            return this.View();
        }

        [Route("departments")]
        public ActionResult DepartmentsPanel()
        {
            var departmentVMs = this.service.GetDepartmentVMs();
            return this.PartialView("Partials/DepartmentsPanel", departmentVMs);
        }

        [Route(@"categories/{departmentId:regex(\d+)}")]
        public ActionResult CategoriesPanel(int departmentId)
        {
            var categoriesVM = this.service.GetDepartmentCategoriesVM(departmentId);
            if (categoriesVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return this.PartialView("Partials/CategoriesPanel", categoriesVM);
        }

        [Route(@"category/{categoryId:regex(\d+)}/subcategories")]
        public ActionResult SubCategoriesPanel(int categoryId)
        {
            var subCategoriesVM = this.service.GetSubCategoryVMs(categoryId);
            if (subCategoriesVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return this.PartialView("Partials/SubCategoriesPanel", subCategoriesVM);
        }

        [HttpGet]
        [Route(@"products-list/{subCategoryId:regex(\d+)}")]
        [Route("products-list")]
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

        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }
    }
}