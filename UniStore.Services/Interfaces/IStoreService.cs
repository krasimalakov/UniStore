namespace UniStore.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Models.BindingModels.Product;
    using Models.EntityModels;
    using Models.ViewModels.Category;
    using Models.ViewModels.Department;
    using Models.ViewModels.Product;
    using Models.ViewModels.ShoppingCard;

    public interface IStoreService
    {
        IEnumerable<DepartmentVM> GetDepartmentVMs();

        DepartmentCategoriesVM GetDepartmentCategoriesVM(int departmentId);

        CategorySubCategoriesVM GetSubCategoryVMs(int categoryId);

        ProductsListVM GetProductsListVM(SearchProductsBM searchBM, string userId);

        ShoppingCart GetUserShoppingCard(string userId);

        ShoppingCardVM GetUserShoppingCardVM(string userId, SearchProductsBM searchProductsBM);

        ShoppingCardVM GetUserFinishOrderVM(string userId, SearchProductsBM searchProductsBM);

        bool AddProductToUserShoppingCard(string userId, int productId);

        bool UpdatePurchaseToUserShoppingCard(string userId, int purchaseId, int quantity);

        bool FinishOrder(string userId, ShoppingCardVM finishOrderVM);
    }
}