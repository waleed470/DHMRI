using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Transaction
    {
        [Key]
        public int Transaction_id { get; set; }
        public int Transaction_item_id { get; set; }
        public string Transaction_item_type { get; set; }
        public string Transaction_Description { get; set; }
        public bool isByCash { get; set; }
        public string BankAccountNo { get; set; }
        public int checkno { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public DateTime Transaction_DateTime { get; set; }
        public int Opening_ClosingDays_id { get; set; }
        public virtual Opening_ClosingDays Opening_ClosingDays { get; set; }
        public bool status { get; set; }
    }
}