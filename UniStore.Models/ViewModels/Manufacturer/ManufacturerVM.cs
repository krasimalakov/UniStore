namespace UniStore.Models.ViewModels.Manufacturer
{
    using System.ComponentModel;

    public class ManufacturerVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Contact information")]
        public string ContactInformation { get; set; }
    }
}