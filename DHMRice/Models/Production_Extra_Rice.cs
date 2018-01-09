using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Production_Extra_Rice
    {
        [Key]
        public int Production_Extra_Rice_id { get; set; }

        public int Rice_Produce_Bags_id { get; set; }
        public virtual Rice_Produce_Bag Rice_Produce_Bag { get; set; }

        public decimal Extra_Rice { get; set; }
    }

}   