namespace UniStore.Models.BindingModels.Manufacturer
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class AddManufacturerBM
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [DisplayName("Contact information")]
        public string ContactInformation { get; set; }
    }
}