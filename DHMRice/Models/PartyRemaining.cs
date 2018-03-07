using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class PartyRemaining
    {
        [Key]
        public int PartyRemaining_Id { get; set; }
        public int Party_Id { get; set; }
        public virtual Party Party { get; set; }
        public string RemainingType { get; set; }
        public bool isPayed { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal UserAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}