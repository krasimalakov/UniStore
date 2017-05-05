namespace UniStore.Models.EntityModels
{
    using System.Collections.Generic;

    public class Category : BaseCategory
    {
        public Category()
        {
            this.SubCategories = new List<SubCategory>();
        }

        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual List<SubCategory> SubCategories { get; set; }
    }
}