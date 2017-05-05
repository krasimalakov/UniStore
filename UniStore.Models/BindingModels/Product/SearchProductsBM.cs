namespace UniStore.Models.BindingModels.Product
{
    public class SearchProductsBM:Pagination
    {
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
