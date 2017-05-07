namespace UniStore.Models.ViewModels.Order
{
    using System.Collections.Generic;
    using User;

    public class OrdersListVM : UserVM
    {
        public IList<OrderVM> Orders { get; set; }

        public Pagination Pagination { get; set; }
    }
}