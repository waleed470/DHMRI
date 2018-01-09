using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Rice_Production
    {
        [Key]
        public int Rice_Production_id { get; set; }

        public String Rice_Production_name { get; set; }

        public String Rice_Production_Code { get; set; }

        public int Packing_Id { get; set; }
        public virtual Packing packing { get; set; }

        public int Rice_Production_RemainingBags { get; set; }

        public DateTime Rice_Production_Date { get; set; }

        public bool Status { get; set; }




    }
}