namespace UniStore.Models.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseCategory
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}