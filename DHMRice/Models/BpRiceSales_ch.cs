using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class BpRiceSales_ch
    {
        [Key]
        public int bprsc_id { get; set; }
        public string bprsc_title { get; set; }
        public int Rice_Production_ProductWorth_id { get; set; }
        public virtual Rice_Production_ProductWorth Rice_Production_ProductWorth { get; set; }
        public int bprsc_qty { get; set; }
        public decimal bprsc_Weight_kg { get; set; }
        public decimal bprsc_Weight_mann { get; set; }
        public decimal bprsc_price { get; set; }
        public int bprsp_id { get; set; }
        public virtual BpRiceSales_pt BpRiceSales_pt { get; set; }
        public bool bprsc_status { get; set; }
    }
}