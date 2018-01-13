using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Packing
    {
        [Key]
        public int Packing_Id { get; set; }

        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int Packing_Type { get; set; }
    }
}