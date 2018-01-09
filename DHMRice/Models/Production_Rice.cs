using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DHMRice.Models
{
    public class Production_Rice
    {
        [Key]
        public int Production_Rice_id { get; set; }

        public int Rice_Produce_Bags_id { get; set; }
        public virtual Rice_Produce_Bag Rice_Produce_Bag { get; set; }

        public int RawRice_id { get; set; }
        public virtual RawRice RawRice { get; set; }

        public decimal Purchase_Rice_Rate { get; set; }

        public int Purchase_Rice_BagsUsed { get; set; }

        public int Packing_Id { get; set; }


        public decimal Total_Weight { get; set; }

        public decimal Total_Worth { get; set; }

        public decimal Market_Worth { get; set; }
        public decimal Market_Rate { get; set; }






    }
}