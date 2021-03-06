﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class RawRice
    {
        [Key]
        public int RawRice_id { get; set; }
      
        public int? Party_Id { get; set; }
        public virtual Party party{ get; set; }
       
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int? Broker_Id { get; set; }
        public virtual Broker broker { get; set; }
       
        public int Rice_category_Id { get; set; }
        public virtual Rice_Category rice_Category { get; set; }
       
        public string Item_Name { get; set; }
      
        public string Item_Code { get; set; }
       
        public int Packing_Id { get; set; }
        public virtual Packing packing { get; set; }
       
        public int Bags_qty { get; set; }
        public int Bags_Sold_qty { get; set; }
        
        public decimal Total_Weight { get; set; }
        public decimal Pb_Weight { get; set; }
       
        public decimal Total_Mann { get; set; }
        //public decimal Payed_Amount { get; set; }
        //public decimal Remaining_Amount { get; set; }
        public bool Pay_CommissionPercentage { get; set; }
        public decimal BrokerCommissionPercentage { get; set; }
        public decimal BrokerCommissionAmount { get; set; }

        public DateTime Date { get; set; }
        public bool Status { get; set; }

    }
}