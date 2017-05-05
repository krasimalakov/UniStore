using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniStore.Models.ViewModels.Invoice
{
    using System.ComponentModel.DataAnnotations;
    using EntityModels;
    using Purchase;

    public class InvoiceVM
    {
        public int Id { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }

        public List<PurchaseVM> Purchases { get; set; }

        public decimal Total { get; set; }
    }
}
