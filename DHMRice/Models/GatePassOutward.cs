using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class GatePassOutward
    {
        public int GatePassOutwardId { get; set; }

        public int RiceTypeId { get; set; }

        public string RiceType { get; set; }

        public string Vehicle_No { get; set; }

        public string Driver_Name { get; set; }

        public int Bility_No { get; set; }

        public string Designation { get; set; }

        public DateTime Date { get; set; }
    }
}