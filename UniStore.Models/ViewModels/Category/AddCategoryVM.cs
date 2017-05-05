namespace UniStore.Models.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;

    public class AddCategoryVM
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }
    }
}