namespace UniStore.Models.ViewModels.SubCategory
{
    using System.Collections.Generic;
    using Product;

    public class SubCategoryProductsVM
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ProductVM> Products { get; set; }

        public int ProductsCount { get; set; }
    }
}