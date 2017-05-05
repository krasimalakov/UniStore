namespace UniStore.Models.ViewModels.Category
{
    using System.Collections.Generic;
    using SubCategory;

    public class CategorySubCategoriesVM
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<SubCategoryVM> SubCategories { get; set; }

        public int ProductsCount { get; set; }
    }
}