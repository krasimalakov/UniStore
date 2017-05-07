namespace UniStore.Models.BindingModels.Order
{
    using System.ComponentModel.DataAnnotations;

    public class DeliveryAddressBM
    {
        [Required]
        [MinLength(15, ErrorMessage = "{0} must be at least {1} symbols long!")]
        public string DeliveryAddress { get; set; }
    }
}