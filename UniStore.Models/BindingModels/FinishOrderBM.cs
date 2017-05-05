namespace UniStore.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Product;

    public class FinishOrderBM
    {
        public SearchProductsBM SearchProductsBM { get; set; }

        [Required]
        [MinLength(15, ErrorMessage = "{0} must be at least {1} symbols long!")]
        public string DeliveryAddress { get; set; }
    }
}