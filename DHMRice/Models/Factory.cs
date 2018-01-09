using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Factory
    {
        [Key]
        public int Factory_Id { get; set; }

        [Required]
        public string Factory_Name { get; set; }

       
        public string Factory_Code { get; set; }

        [Required]
        public string Factory_Number { get; set; }

        [Required]
        public string Factory_BankName { get; set; }

        [Required]
        public string Factory_ACcountNo { get; set; }

        [Required]
        public string Factory_Address { get; set; }

        public bool Status { get; set; }
    }
}