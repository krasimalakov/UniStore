namespace UniStore.Models.BindingModels.SubCategory
{
    using System.ComponentModel.DataAnnotations;

    public class AddSubCategoryBM
    {
        public int DepartmentId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}