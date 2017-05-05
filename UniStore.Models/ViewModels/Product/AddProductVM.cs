namespace UniStore.Models.ViewModels.Product
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class AddProductVM
    {
        public int DepartmentId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Manufacturer")]
        public int ManufacturerId { get; set; }

        public string ImageUrl { get; set; }

        [DisplayName("Set image")]
        public HttpPostedFileBase Image { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public IEnumerable<SelectListItem> Manufacturers { get; set; }
    }
}