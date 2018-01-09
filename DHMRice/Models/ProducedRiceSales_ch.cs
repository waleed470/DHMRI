using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class ProducedRiceSales_ch
    {
        [Key]
        public int prsc_id { get; set; }
        public string prsc_title { get; set; }
        public int Rice_Production_id { get; set; }
        public virtual Rice_Production Rice_Production { get; set; }
        public int prsc_qty { get; set; }
        public decimal prsc_Weight_kg { get; set; }
        public decimal prsc_Weight_mann { get; set; }
        public decimal prsc_price { get; set; }
        public int prsp_id { get; set; }
        public virtual ProducedRiceSales_pt ProducedRiceSales_pt { get; set; }
        public bool prsc_status { get; set; }
    }
}