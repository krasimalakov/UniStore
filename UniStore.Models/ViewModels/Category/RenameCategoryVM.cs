namespace UniStore.Models.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;

    public class RenameCategoryVM
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public string Name { get; set; }
    }
}