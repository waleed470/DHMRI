using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DHMRice.Models
{
    public class Rice_Production_ShortFall
    {
        [Key]
        public int Rice_Production_ShortFall_id { get; set; }

        public int Rice_Produce_Bags_id { get; set; }
        public virtual Rice_Produce_Bag Rice_Produce_Bag { get; set; }

        public string Rice_Production_ShortFall_name { get; set; }
      

        public decimal Rice_Production_ShortFall_Amount { get; set; }
    }
}