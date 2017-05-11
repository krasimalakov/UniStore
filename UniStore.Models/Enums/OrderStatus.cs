namespace UniStore.Models.Enums
{
    using System.ComponentModel;

    public enum OrderStatus
    {
        [Description("The order was accepted")]
        Аccepted=0,

        [Description("The order is approved")]
        Approved=1,

        [Description("The order has been sent")]
        Sent=2,

        [Description("The order has been successfully completed")]
        Completed = 3,

        [Description("The order has been canceled by the customer")]
        Canceled=11,

        [Description("The order was rejected by the seller")]
        Rejected=12,

        [Description("The order is returned")]
        Returned = 13
    }
}