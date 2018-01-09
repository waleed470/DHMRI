using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Rice_Category
    {
        [Key]
        public int Rice_category_Id { get; set; }

        public string Rice_Category_Name { get; set; }

        public bool Status { get; set; }

    }
}