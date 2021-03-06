﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class ShopRiceSales_pt
    {
         [Key]
        public int srsp_id { get; set; }
        public string srsp_Title { get; set; }
        public DateTime srsp_date { get; set; }
        public int Customer_Id { get; set; }
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Customer Customer { get; set; }
        public int Shop_Id { get; set; }
        public virtual Shop Shop{ get; set; }
        public decimal srsp_TotalWeight_KG { get; set; }
        public decimal srsp_TotalWeight_Mann { get; set; }
        public decimal srsp_Total_Amount { get; set; }
        public bool srsp_status { get; set; }
    }
}