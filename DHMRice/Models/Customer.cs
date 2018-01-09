using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Customer
    {
        [Key]
        public int Customer_Id { get; set; }

        [Required]
        public string Customer_Name { get; set; }

        [Required]
        public string Customer_Code { get; set; }

        [Required]
        public string Customer_MobileNo { get; set; }

        [Required]
        public string Customer_BankName { get; set; }

        [Required]
        public string Customer_ACcountNo { get; set; }

        [Required]
        public string Customer_Address { get; set; }

        public bool Status { get; set; }
    }
}