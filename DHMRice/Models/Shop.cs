using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Shop
    {
        [Key]
        public int Shop_Id { get; set; }

        [Required]
        public string Shop_Name { get; set; }
       
        public string Shop_Code { get; set; }
    
        public string Shop_MobileNo { get; set; }

        public string Shop_Address { get; set; }

        public bool Status { get; set; }
    }
}