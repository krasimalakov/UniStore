namespace UniStore.Services.Interfaces
{
    using Models.BindingModels.SubCategory;
    using Models.ViewModels.Category;
    using Models.ViewModels.SubCategory;

    public interface ISubCategoriesService
    {
        CategorySubCategoriesVM GetCategorySubCategoriesVM(int departmentId, int categoryId);

        bool IsExistSubCategoryWithName(int departmentId, int categoryId, string name);

        bool IsExistOtherCategoryWithName(int departmentId, int categoryId, int id, string name);

        bool AddSubCategory(int departmentId, int categoryId, AddSubCategoryBM categoryBM);

        RenameSubCategoryVM GetRenameSubCategoryVM(int departmentId, int categoryId, int id);

        bool RenameCategory(int departmentId, int categoryId, int id, string name);

        bool RemoveSubCategory(int departmentId, int categoryId, int id);

        DetailsSubCategoryVM GetDetailsSubCategoryVM(int departmentId, int categoryId, int id);
    }
}