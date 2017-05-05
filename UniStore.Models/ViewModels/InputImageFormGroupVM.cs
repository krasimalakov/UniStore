namespace UniStore.Models.ViewModels
{
    using System.Web;

    public class InputImageFormGroupVM
    {
        public string ImageUrl { get; set; }

        public HttpPostedFileBase Image { get; set; }
    }
}