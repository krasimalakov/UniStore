namespace UniStore.Services.Implementation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using AutoMapper;
    using Data.UnitOfWork;
    using Interfaces;
    using Models;
    using Models.BindingModels.Product;
    using Models.EntityModels;
    using Models.ViewModels.Product;
    using Models.ViewModels.SubCategory;

    public class ProductsService : BaseService, IProductsService
    {
        public ProductsService(IUniStoreContext context)
            : base(context)
        {
        }

        public SubCategoryProductsVM GetSubCategoryProductsVM(int subCategoryId)
        {
            var subCategory = this.Context.SubCategories.All()
                .FirstOrDefault(sc => sc.Id == subCategoryId);

            if (subCategory == null)
            {
                return null;
            }

            var vm= Mapper.Map<SubCategoryProductsVM>(subCategory);

            return vm;
        }

        public bool IsExistProductWithName(AddProductBM productBM)
        {
            var products = this.Context.Departments.Find(productBM.DepartmentId)
                ?.Categories.FirstOrDefault(c => c.Id == productBM.CategoryId)
                ?.SubCategories.FirstOrDefault(sc => sc.Id == productBM.SubCategoryId)
                ?.Products;
            if (products == null)
            {
                return false;
            }

            return products.Any(p => p.Name.Equals(productBM.Name));
        }

        public bool IsExistOtherProductWithName(EditProductBM productBM)
        {
            var result = this.Context.Departments.Find(productBM.DepartmentId)
                ?.Categories.FirstOrDefault(c => c.Id == productBM.CategoryId)
                ?.SubCategories.FirstOrDefault(sc => sc.Id == productBM.SubCategoryId)
                ?.Products.Any(p => p.Name.Equals(productBM.Name) && p.Id != productBM.Id);

            return result ?? false;
        }

        public bool AddProduct(AddProductBM productBM)
        {
            var subCategory = this.Context.Departments.Find(productBM.DepartmentId)
                ?.Categories.FirstOrDefault(c => c.Id == productBM.CategoryId)
                ?.SubCategories.FirstOrDefault(sc => sc.Id == productBM.SubCategoryId);
            var manufakcturer = this.Context.Manufacturers.Find(productBM.ManufacturerId);

            if (subCategory == null || manufakcturer == null)
            {
                return false;
            }

            var product = Mapper.Map<Product>(productBM);
            product.Manufacturer = manufakcturer;
            subCategory.Products.Add(product);
            this.Context.SaveChanges();

            if (productBM.Image != null && productBM.Image.ContentLength > 0)
            {
                SaveProductImage(product, productBM.Image);
                this.Context.SaveChanges();
            }


            return true;
        }

        public bool EditProduct(EditProductBM productBM)
        {
            var product = this.GetProduct(productBM.Id);

            var manufakcturer = this.Context.Manufacturers.Find(productBM.ManufacturerId);

            if (product == null || manufakcturer == null)
            {
                return false;
            }

            if (productBM.Image != null && productBM.Image.ContentLength > 0)
            {
                SaveProductImage(product, productBM.Image);
            }


            product.Name = productBM.Name;
            product.Description = productBM.Description;
            product.Price = productBM.Price;
            product.Quantity = productBM.Quantity;
            product.Manufacturer = manufakcturer;
            this.Context.SaveChanges();

            return true;
        }

        public Product GetProduct(int id)
        {
            return this.Context.Products.Find(id);
        }

        public DetailsProductVM GetDetailsProductVM(int id)
        {
            var product = this.GetProduct(id);
            if (product == null)
            {
                return null;
            }

            var productVM = Mapper.Map<DetailsProductVM>(product);

            return productVM;
        }

        public AddProductVM GetAddProductVM(int subCategoryId)
        {
            var manufacturersSelectList = this.GetManufacturersSelectList();

            var subCategory = this.Context.SubCategories.Find(subCategoryId);
            if (subCategory == null)
            {
                return null;
            }

            var productVM = new AddProductVM
            {
                DepartmentId = subCategory.Category.Department.Id,
                CategoryId = subCategory.Category.Id,
                SubCategoryId = subCategoryId,
                Manufacturers = manufacturersSelectList
            };

            return productVM;
        }

        public EditProductVM GetEditProductVM(int id)
        {
            var product = this.GetProduct(id);
            if (product == null)
            {
                return null;
            }

            var productVM = Mapper.Map<EditProductVM>(product);

            var manufacturersSelectList = this.GetManufacturersSelectList(productVM.ManufacturerId);
            productVM.DepartmentId = product.SubCategory.Category.Department.Id;
            productVM.CategoryId = product.SubCategory.Category.Id;
            productVM.Manufacturers = manufacturersSelectList;

            return productVM;
        }

        public IEnumerable<SelectListItem> GetManufacturersSelectList(int manufacturerId = -1)
        {
            var manufacturers = this.Context.Manufacturers.All().OrderBy(m => m.Name).ToList();
            var manufacturersSelectList = new List<SelectListItem>(
                manufacturers.Select(
                    m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name,
                        Selected = m.Id == manufacturerId
                    }));

            return manufacturersSelectList;
        }

        private static void SaveProductImage(Product product, HttpPostedFileBase image)
        {
            var fileName = Path.GetFileName(image.FileName);
            if (fileName != null)
            {
                fileName = ProductImageFileName(product, fileName);
                var path = Path.Combine(HostingEnvironment.MapPath(Constants.ImagePath), fileName);
                image.SaveAs(path);
                product.ImageUrl = (Constants.ImagePath + fileName).Substring(1);
            }
        }

        private static string ProductImageFileName(Product product, string fileName)
        {
            return $"{product.SubCategory.Category.Department.Id}-" +
                   $"{product.SubCategory.Category.Id}-{product.SubCategory.Id}-{product.Id}" +
                   fileName.Substring(fileName.LastIndexOf('.'));
        }

        public Product RemoveProduct(int id)
        {
            var product = this.GetProduct(id);
            if (product == null)
            {
                return null;
            }

            this.Context.Products.Remove(product);
            this.Context.SaveChanges();

            return product;
        }
    }
}