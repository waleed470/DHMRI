using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class BpRiceSales_pt
    {
        [Key]
        public int bprsp_id { get; set; }
        public string bprsp_Title { get; set; }
        public DateTime bprsp_date { get; set; }
        public int Party_Id { get; set; }
        public virtual Party Party { get; set; }

        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
       
        public decimal bprsp_TotalWeight_KG { get; set; }
        public decimal bprsp_TotalWeight_Mann { get; set; }
        public decimal bprsp_Total_Amount { get; set; }
        public int Carriage { get; set; }
        public int Labour { get; set; }
        public bool bprsp_status { get; set; }
    }
}