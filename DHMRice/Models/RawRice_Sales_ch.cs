using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class RawRice_Sales_ch
    {
        [Key]
        public int rsc_id { get; set; }
        public string rsc_title { get; set; }
        public int RawRice_id { get; set; }
        public virtual RawRice RawRice { get; set; }
        public int rsc_qty { get; set; }
        public decimal rsc_Weight_kg { get; set; }
        public decimal rsc_Weight_mann { get; set; }
        public decimal rsc_price { get; set; }
        public int rsp_id { get; set; }
        public virtual RawRice_Sales_pt RawRice_Sales_pt { get; set; }
        public bool rsc_status { get; set; }

    }
}