namespace UniStore.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Models.BindingModels.Product;
    using Models.EntityModels;
    using Models.ViewModels.Product;
    using Models.ViewModels.SubCategory;

    public interface IProductsService
    {
        SubCategoryProductsVM GetSubCategoryProductsVM(int subCategoryId);

        bool IsExistProductWithName(AddProductBM productBM);

        bool IsExistOtherProductWithName(EditProductBM productBM);

        bool AddProduct(AddProductBM productBM);

        bool EditProduct(EditProductBM productBM);

        Product RemoveProduct(int id);

        Product GetProduct(int id);

        DetailsProductVM GetDetailsProductVM(int id);

        AddProductVM GetAddProductVM(int subCategoryId);

        EditProductVM GetEditProductVM(int id);

        IEnumerable<SelectListItem> GetManufacturersSelectList(int manufacturerId = -1);

    }
}