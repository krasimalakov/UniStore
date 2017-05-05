namespace UniStore.Models.ViewModels.Department
{
    using System.Collections.Generic;
    using Category;

    public class DepartmentCategoriesVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<CategoryVM> Categories { get; set; }

        public int ProductsCount { get; set; }
    }
}