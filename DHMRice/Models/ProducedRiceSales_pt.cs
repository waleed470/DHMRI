using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class ProducedRiceSales_pt
    {
         [Key]
        public int prsp_id { get; set; }
        public string prsp_Title { get; set; }
        public DateTime prsp_date { get; set; }
        public int Party_Id { get; set; }
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Party Party { get; set; }
        public decimal prsp_TotalWeight_KG { get; set; }
        public decimal prsp_TotalWeight_Mann { get; set; }
        public decimal prsp_Total_Amount { get; set; }
        public bool prsp_status { get; set; }
    }
}