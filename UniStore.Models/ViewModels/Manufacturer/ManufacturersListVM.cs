namespace UniStore.Models.ViewModels.Manufacturer
{
    using System.Collections.Generic;

    public class ManufacturersListVM
    {
        public IList<ManufacturerVM> Manufacturers { get; set; }

        public Pagination Pagination { get; set; }
    }
}