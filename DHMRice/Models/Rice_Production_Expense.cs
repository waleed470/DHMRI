using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DHMRice.Models
{
    public class Rice_Production_Expense
    {
        [Key]
        public int Rice_Production_Expense_id { get; set; }

        public int Rice_Produce_Bags_id { get; set; }
        public virtual Rice_Produce_Bag Rice_Produce_Bag { get; set; }

        public string Rice_Production_Expense_name { get; set; }


        public decimal Rice_Production_Expense_Amount { get; set; }
    }
}