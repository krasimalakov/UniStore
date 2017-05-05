namespace UniStore.Services.Interfaces
{
    using Models.BindingModels.Category;
    using Models.BindingModels.Departments;
    using Models.ViewModels.Category;
    using Models.ViewModels.Department;

    public interface ICategoriesService
    {
        bool IsExistCategoryWithName(int departmentId, string name);

        bool IsExistOtherCategoryWithName(int departmentId, int id, string name);

        DepartmentCategoriesVM GetDepartmentCategoriesVM(int id);

        bool AddCategory(int departmentId, CategoryBM categoryBM);

        bool RemoveCategory(int departmentId, int id);

        RenameCategoryVM GetRenameCategoryVM(int departmentId, int id);

        bool RenameCategory(int departmentId, int id, string name);

        DetailsCategoryVM GetDetailsCategoryVM(int departmentId, int categoryId);
    }
}