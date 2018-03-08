using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Pricing
    {
        [Key]
        public int Pricing_id { get; set; }
        public int item_id { get; set; }
        public string item_Type { get; set; }
        public decimal Pricing_Rate_kg { get; set; }
        public decimal Pricing_Rate_Maan { get; set; }
        public decimal Pricing_Total { get; set; }
        public decimal PerBagPrice { get; set; }
        public decimal PerBagMarketPrice { get; set; }
        public decimal Pricing_NetTotal { get; set; }
        public DateTime Pricing_Date { get; set; }
        public DateTime Pricing_ModifiedDate { get; set; }
        public bool Status { get; set; }
    }
}