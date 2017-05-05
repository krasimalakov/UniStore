namespace UniStore.Models.EntityModels
{
    using System.Collections.Generic;

    public class Department : BaseCategory
    {
        public Department()
        {
            this.Categories = new List<Category>();
        }

        public virtual List<Category> Categories { get; set; }
    }
}