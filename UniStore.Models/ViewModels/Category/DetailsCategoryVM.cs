namespace UniStore.Models.ViewModels.Category
{
    using System.Collections.Generic;
    using SubCategory;

    public class DetailsCategoryVM : CategoryVM
    {
        public string DepartmentName { get; set; }
        public virtual IEnumerable<SubCategoryVM> SubCategories { get; set; }
    }
}