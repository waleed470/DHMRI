using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class ShopStock
    {
        [Key]
        public int ShopStockId { get; set; }


        public string srsc_title { get; set; }

        public int? Rice_Production_id { get; set; }
        public virtual Rice_Production Rice_Production { get; set; }

        public int? srsp_id { get; set; }
        public virtual ShopStock_pt ShopStock_pt { get; set; }

        public int Qty { get; set; }

        public int packing_type { get; set; }


        public int SoldQty { get; set; }

        public decimal PerBagPrice { get; set; }



        public decimal SumWeight_KG { get; set; }
        public decimal TotalWeight_KG { get; set; }
        public decimal SumWeight_Mann { get; set; }
        public decimal TotalWeight_Mann { get; set; }
        public decimal Total_Amount { get; set; }

        public decimal NetTotal { get; set; }

        //public DateTime ShopDate { get; set; }

        public bool Status { get; set; }

    }
}