namespace UniStore.Services.Implementation
{
    using System.Linq;
    using AutoMapper;
    using Data.UnitOfWork;
    using Interfaces;
    using Models.BindingModels.Category;
    using Models.BindingModels.Departments;
    using Models.EntityModels;
    using Models.ViewModels.Category;
    using Models.ViewModels.Department;

    public class CategoriesService : BaseService, ICategoriesService
    {
        public CategoriesService(IUniStoreContext context)
            : base(context)
        {
        }

        public bool IsExistCategoryWithName(int departmentId, string name)
        {
            return this.Context.Departments.Find(departmentId)
                .Categories.Any(c => string.Equals(c.Name, name));
        }

        public bool IsExistOtherCategoryWithName(int departmentId, int id, string name)
        {
            return this.Context.Departments.Find(departmentId)
                .Categories.Any(c => string.Equals(c.Name, name) && c.Id != id);
        }

        public DepartmentCategoriesVM GetDepartmentCategoriesVM(int id)
        {
            var department = this.Context.Departments.Find(id);
            if (department == null)
            {
                return null;
            }

            var departmentVM = Mapper.Map<DepartmentCategoriesVM>(department);

            return departmentVM;
        }

        public bool AddCategory(int departmentId, CategoryBM categoryBM)
        {
            var department = this.Context.Departments.Find(departmentId);
            if (department == null)
            {
                return false;
            }

            var category = Mapper.Map<Category>(categoryBM);
            department.Categories.Add(category);
            this.Context.SaveChanges();

            return true;
        }

        public bool RemoveCategory(int departmentId, int id)
        {
            var department = this.Context.Departments.Find(departmentId);

            var category = department?.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return false;
            }

            this.Context.Categories.Remove(category);
            this.Context.SaveChanges();

            return true;
        }

        public RenameCategoryVM GetRenameCategoryVM(int departmentId, int id)
        {
            var category = this.Context
                .Departments.Find(departmentId)
                ?
                .Categories.FirstOrDefault(c => c.Id == id);
            var vm = Mapper.Map<RenameCategoryVM>(category);

            return vm;
        }

        public bool RenameCategory(int departmentId, int id, string name)
        {
            var department = this.Context.Departments.Find(departmentId);

            var category = department?.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return false;
            }

            category.Name = name;
            this.Context.SaveChanges();

            return true;
        }

        public DetailsCategoryVM GetDetailsCategoryVM(int departmentId, int categoryId)
        {
            var category = this.Context
                .Departments.Find(departmentId)?
                .Categories.FirstOrDefault(c => c.Id == categoryId);
            var categoryVM= Mapper.Map<DetailsCategoryVM>(category);

            return categoryVM;
        }
    }
}