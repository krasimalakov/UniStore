namespace UniStore.Models.ViewModels.Department
{
    using System.Collections.Generic;
    using Category;

    public class DetailsDepartmentVM : DepartmentVM
    {
        public virtual IEnumerable<CategoryVM> Categories { get; set; }
    }
}