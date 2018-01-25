using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class ShopRiceSales_ch
    {
        [Key]
        public int srsc_id { get; set; }
        public string srsc_title { get; set; }
        public int ShopStockId { get; set; }
        public virtual ShopStock ShopStock { get; set; }
        public int srsc_qty { get; set; }
        public decimal srsc_Weight_kg { get; set; }
        public decimal srsc_Weight_mann { get; set; }
        public decimal srsc_price { get; set; }
        public int srsp_id { get; set; }
        public virtual  ShopRiceSales_pt ShopRiceSales_pt { get; set; }
        public bool srsc_status { get; set; }
    }
}