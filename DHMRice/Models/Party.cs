using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Party
    {
        [Key]
        public int Party_Id { get; set; }

        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string Party_Name { get; set; }

       
        public string Party_Code { get; set; }

        [Required]
        public string Party_MobileNo { get; set; }

       
        public string Party_BankName { get; set; }

       
        public string Party_ACcountNo { get; set; }

      
        public string Party_Address { get; set; }

        public bool Status { get; set; }
    }
}