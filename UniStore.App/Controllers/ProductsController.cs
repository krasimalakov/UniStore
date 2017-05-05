namespace UniStore.App.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using Attributes;
    using AutoMapper;
    using Data.UnitOfWork;
    using Models.BindingModels.Product;
    using Models.EntityModels;
    using Models.Enums;
    using Models.ViewModels.Product;
    using Services.Interfaces;

    [AuthorizeInRole(AppRole.Administrator)]
    [RoutePrefix("products")]
    public class ProductsController : BaseController
    {
        private readonly IProductsService service;

        public ProductsController(IUniStoreContext context, IProductsService service)
            : base(context)
        {
            this.service = service;
        }

        public ProductsController(IUniStoreContext context, IProductsService service, User user)
            : base(context, user)
        {
            this.service = service;
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{subCategoryId:regex(\d+)}")]
        public ActionResult ProductsList(int subCategoryId)
        {
            var subCategoryVM = this.service.GetSubCategoryProductsVM(subCategoryId);
            this.Response.AddHeader("Location", "/departments");
            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView(subCategoryVM);
            }

            return this.View(subCategoryVM);
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{subCategoryId:regex(\d+)}/add")]
        public ActionResult Add(int subCategoryId)
        {
            AddProductVM addProductVM = this.service.GetAddProductVM(subCategoryId);

            if (addProductVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView(addProductVM);
            }

            return this.View(addProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{subCategoryId:regex(\d+)}/add")]
        public ActionResult Add(AddProductBM productBM)
        {
            if (this.service.IsExistProductWithName(productBM))
            {
                this.ModelState.AddModelError(
                    nameof(productBM.Name),
                    $"The product with name {productBM.Name} already exist in subcategory!");
            }

            if (!this.ModelState.IsValid)
            {
                var productVM = Mapper.Map<AddProductVM>(productBM);
                productVM.Manufacturers =
                    this.service.GetManufacturersSelectList(productVM.ManufacturerId);

                return this.View(productVM);
            }

            if (!this.service.AddProduct(productBM))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction(
                "ProductsList",
                new
                {
                    productBM.DepartmentId,
                    productBM.CategoryId,
                    productBM.SubCategoryId
                });
        }

        [HttpGet]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{subCategoryId:regex(\d+)}/{id:regex(\d+)}/edit")]
        public ActionResult Edit(int id)
        {
            EditProductVM productVM = this.service.GetEditProductVM(id);
            if (productVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView(productVM);
            }

            return this.View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{subCategoryId:regex(\d+)}/{id:regex(\d+)}/edit")]
        public ActionResult Update(EditProductBM productBM, int id)
        {
            if (this.service.IsExistOtherProductWithName(productBM))
            {
                this.ModelState.AddModelError(
                    nameof(productBM.Name),
                    $"Other product with name {productBM.Name} already exist in subcategory!");
            }

            if (!this.ModelState.IsValid)
            {
                var productVM = Mapper.Map<EditProductVM>(productBM);
                productVM.Manufacturers =
                    this.service.GetManufacturersSelectList(productVM.ManufacturerId);

                return this.View("Edit", productVM);
            }


            if (!this.service.EditProduct(productBM))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return this.RedirectToAction(
                "ProductsList",
                new
                {
                    productBM.DepartmentId,
                    productBM.CategoryId,
                    productBM.SubCategoryId
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(@"{departmentId:regex(\d+)}/{categoryId:regex(\d+)}/{subCategoryId:regex(\d+)}/{id:regex(\d+)}/delete")]
        public ActionResult Delete(int id)
        {
            var product = this.service.RemoveProduct(id);
            if (product==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return this.RedirectToAction("ProductsList", 
                new { subCategoryId=product.SubCategory.Id });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(@"{id:regex(\d+)}/details")]
        public ActionResult Details(int id)
        {
            var productVM = this.service.GetDetailsProductVM(id);
            if (productVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (this.Request.IsAjaxRequest())
            {
                return this.PartialView("Partials/Details", productVM);
            }

            return this.View("Partials/Details", productVM);
        }
    }
}