namespace UniStore.Models.BindingModels.Product
{
    using System.Web.Mvc;

    public class SearchProductsBM : Pagination
    {
        public int DepartmentId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public SelectList OrderList { get; set; }

        public SelectList OrderByList { get; set; }
    }
}