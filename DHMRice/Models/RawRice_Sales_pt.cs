using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class RawRice_Sales_pt
    {
        [Key]
        public int rsp_id { get; set; }
        public string rsp_Title { get; set; }
        public DateTime rsp_date { get; set; }
        public int Party_Id { get; set; }
        public virtual Party Party { get; set; }
        public decimal rsp_TotalWeight_KG { get; set; }
        public decimal rsp_TotalWeight_Mann { get; set; }
        public decimal rsp_Total_Amount { get; set; }
        public bool rsp_status { get; set; }
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}