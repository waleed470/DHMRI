using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DHMRice.ViewModels
{
    public class ProductionEditViewModel
    {
        public Rice_Production Rice_Productions { get; set; }
        
        public Pricing Pricings { get; set; }
        public IEnumerable<RawRice> RawRices { get; set; }
    }
}