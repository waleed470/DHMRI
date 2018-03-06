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
        public int Rice_Production_id { get; set; }
        public virtual Rice_Production Rice_Production { get; set; }
        public int bprsc_qty { get; set; }
        public decimal bprsc_Weight_kg { get; set; }
        public decimal bprsc_Weight_mann { get; set; }
        public decimal bprsc_price { get; set; }
        public int bprsp_id { get; set; }
        public virtual ProducedRiceSales_pt ProducedRiceSales_pt { get; set; }
        public bool bprsc_status { get; set; }
    }
}