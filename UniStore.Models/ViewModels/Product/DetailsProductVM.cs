namespace UniStore.Models.ViewModels.Product
{
    public class DetailsProductVM : ProductVM
    {
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }
    }
}