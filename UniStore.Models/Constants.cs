namespace UniStore.Models
{
    public static class Constants
    {
        public const string LayoutPath = "~/Views/Shared/_Layout.cshtml";

        public const string AjaxLoadingId = "ajax-indicator";
        public const int LoadingElementDuration = 400;

        public const string ContainerBodyId = "container-body";

        public const string ContainerStoreId = "store";
        public const string ContainerStorePanelId = "store-panel";
        public const string ContainerStoreProductsId = "store-products";

        public const string ContainerOrderListId = "orders";

        public const string ImagePath = "~/Content/Images/";
        public const string DefaultImage = "/Content/Images/no-image.jpg";

        public static readonly string[] OrderBy = { "Last added", "Name", "Price" };
        public static readonly string[] Order = { "Ascending", "Descending" };
    }
}