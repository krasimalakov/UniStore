namespace UniStore.Models.ViewModels.SubCategory
{
    using System.Collections.Generic;
    using Product;

    public class DetailsSubCategoryVM:SubCategoryVM
    {
        public int DepartmentId { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<ProductVM> Products { get; set; }
    }
}