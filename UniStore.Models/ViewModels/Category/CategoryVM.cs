namespace UniStore.Models.ViewModels.Category
{
    public class CategoryVM
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public int SubCategoriesCount { get; set; }

        public int ProductsCount { get; set; }
    }
}