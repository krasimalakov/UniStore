namespace UniStore.Models.EntityModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Manufacturer
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [DisplayName("Contact information")]
        public string ContactInformation { get; set; }
    }
}