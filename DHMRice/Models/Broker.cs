using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Broker
    {
        [Key]
        public int Broker_Id { get; set; }

        [Required]
        public string Broker_Name { get; set; }

        
        public string Broker_Code { get; set; }

       
        public string Broker_MobileNo { get; set; }

       
        public string Broker_BankName { get; set; }

        
        public string Broker_ACcountNo { get; set; }

        
        public string Broker_Address { get; set; }

        public bool Status { get; set; }
    }
}