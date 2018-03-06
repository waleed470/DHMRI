using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Rice_Production_ProductWorth
    {
        [Key]
        public int Rice_Production_ProductWorth_id { get; set; }

        public int Rice_Produce_Bags_id { get; set; }
        public virtual Rice_Produce_Bag Rice_Produce_Bag { get; set; }

        public string Rice_Production_ProductWorth_name { get; set; }

        public decimal Rice_Production_ProductWorth_Qty { get; set; }


        public decimal Rice_Production_ProductWorth_Amount { get; set; }

        public decimal Rice_Production_ProductWorth_PBA { get; set; }
    }
}