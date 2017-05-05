namespace UniStore.Services.Implementation
{
    using System.Linq;
    using AutoMapper;
    using Data;
    using Data.UnitOfWork;
    using Interfaces;
    using Models.BindingModels.SubCategory;
    using Models.EntityModels;
    using Models.ViewModels.Category;
    using Models.ViewModels.SubCategory;

    public class SubCategoriesService : BaseService, ISubCategoriesService
    {
        public SubCategoriesService(IUniStoreContext context)
            : base(context)
        {
        }

        public CategorySubCategoriesVM GetCategorySubCategoriesVM(int departmentId, int categoryId)
        {
            var category = this.Context.Categories.Find(categoryId);
            if (category.Department.Id != departmentId)
            {
                return null;
            }

            var categoryVM = Mapper.Map<CategorySubCategoriesVM>(category);

            return categoryVM;
        }

        public bool IsExistSubCategoryWithName(int departmentId, int categoryId, string name)
        {
            return this.Context
                       .Departments.Find(departmentId)
                       .Categories.FirstOrDefault(c => c.Id == categoryId)
                       ?
                       .SubCategories.Any(sc => sc.Name.Equals(name)) ?? false;
        }

        public bool IsExistOtherCategoryWithName(int departmentId, int categoryId, int id, string name)
        {
            return this.Context
                       .Departments.Find(departmentId)
                       .Categories.FirstOrDefault(c => c.Id == categoryId)
                       ?
                       .SubCategories.Any(sc => sc.Name.Equals(name) && sc.Id != id) ?? false;
        }

        public bool AddSubCategory(int departmentId, int categoryId, AddSubCategoryBM categoryBM)
        {
            var category = this.Context
                .Departments.Find(departmentId)
                .Categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
            {
                return false;
            }

            category.SubCategories.Add(new SubCategory { Name = categoryBM.Name });
            this.Context.SaveChanges();

            return true;
        }

        public RenameSubCategoryVM GetRenameSubCategoryVM(int departmentId, int categoryId, int id)
        {
            var subCategory = this.GetSubCategory(departmentId, categoryId, id);
            var vm = Mapper.Map<RenameSubCategoryVM>(subCategory);

            return vm;
        }

        public bool RenameCategory(int departmentId, int categoryId, int id, string name)
        {
            var subCategory = this.GetSubCategory(departmentId, categoryId, id);
            if (subCategory == null)
            {
                return false;
            }

            subCategory.Name = name;
            this.Context.SaveChanges();

            return true;
        }

        public bool RemoveSubCategory(int departmentId, int categoryId, int id)
        {
            var subCategory = this.GetSubCategory(departmentId, categoryId, id);
            if (subCategory == null)
            {
                return false;
            }

            this.Context.SubCategories.Remove(subCategory);
            this.Context.SaveChanges();

            return true;
        }

        public DetailsSubCategoryVM GetDetailsSubCategoryVM(int departmentId, int categoryId, int id)
        {
            var subCategory = this.GetSubCategory(departmentId, categoryId, id);
            return Mapper.Map<DetailsSubCategoryVM>(subCategory);
        }

        private SubCategory GetSubCategory(int departmentId, int categoryId, int id)
        {
            return this.Context.Departments.Find(departmentId)
                ?.Categories.FirstOrDefault(c => c.Id == categoryId)
                ?.SubCategories.FirstOrDefault(sc => sc.Id == id);
        }
    }
}