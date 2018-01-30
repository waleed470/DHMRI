using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class SaleInvoice
    {
        [Key]
        public int SaleInvoice_id { get; set; }
        public int SaleInvoice_no { get; set; }
        public int Sale_id { get; set; }
        public string SaleInvoice_type { get; set; }
    }
}