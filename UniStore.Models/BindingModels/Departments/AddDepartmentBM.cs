namespace UniStore.Models.BindingModels.Departments
{
    using System.ComponentModel.DataAnnotations;

    public class AddDepartmentBM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}