using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class GatePassInwared
    {
        public int GatePassInwaredId { get; set; }

        public int RawRice_id { get; set; }
        public virtual RawRice RawRice { get; set; }

        public string Vehicle_No { get; set; }

        public string Driver_Name { get; set; }

        public int Bility_No { get; set; }

        public string Purchased_By { get; set; }

        public string Designation { get; set; }

        public DateTime Date { get; set; }

    }
}