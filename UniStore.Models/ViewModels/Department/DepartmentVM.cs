namespace UniStore.Models.ViewModels.Department
{
    public class DepartmentVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoriesCount { get; set; }

        public int ProductsCount { get; set; }
    }
}