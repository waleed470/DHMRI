using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Opening_ClosingDays
    {
        [Key]
        public int Opening_ClosingDays_id { get; set; }
        public decimal Opening_Balance { get; set; }
        public decimal Closing_Balance { get; set; }
        public DateTime Date { get; set; }
        public bool isClosed { get; set; }
        public bool status { get; set; }
    }
}