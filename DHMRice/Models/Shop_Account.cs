using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Shop_Account
    {
        [Key]
        public int Shop_AccountId { get; set; }

        public int Shop_Id { get; set; }
        public virtual Shop Shop { get; set; }

        [Required]
        public string Shop_BankName { get; set; }

        [Required]
        public string Shop_ACcountNo { get; set; }

        public bool Status { get; set; }
    }
}