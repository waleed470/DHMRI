using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Transaction_Shop
    {
        [Key]
        public int Transaction_Shop_id { get; set; }
        public int Transaction_Shop_item_id { get; set; }
        public string Transaction_Shop_item_type { get; set; }
        public string Transaction_Shop_Description { get; set; }
        public bool isByCash { get; set; }
        public string BankAccountNo { get; set; }
        public int checkno { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public DateTime Transaction_Shop_DateTime { get; set; }
        public int Opening_ClosingDays_Shop_id { get; set; }
        public virtual Opening_ClosingDays_Shop Opening_ClosingDays_Shop { get; set; }
        public bool status { get; set; }
    }
}