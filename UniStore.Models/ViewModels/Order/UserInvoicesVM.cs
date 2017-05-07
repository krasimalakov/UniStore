namespace UniStore.Models.ViewModels.Order
{
    using System.Collections.Generic;
    using User;

    public class UserOrdersVM : UserVM
    {
        public List<OrderVM> Orders { get; set; }
    }
}