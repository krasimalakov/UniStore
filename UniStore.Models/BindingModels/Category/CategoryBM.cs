namespace UniStore.Models.BindingModels.Category
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryBM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}