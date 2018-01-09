using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class RawRiceExpense
    {
        [Key]
        public int RawRiceExpense_id { get; set; }        
        public string RawRiceExpense_Name { get; set; }
        public decimal RawRiceExpense_Amount { get; set; }
        public int RawRice_id { get; set; }
        public virtual RawRice RawRice { get; set; }
    }
}