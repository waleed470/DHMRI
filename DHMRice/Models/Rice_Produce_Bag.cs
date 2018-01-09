using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DHMRice.Models
{
    public class Rice_Produce_Bag
    {
        [Key]
        public int Rice_Produce_Bags_id { get; set; }

        public int Rice_Production_id { get; set; }
        public virtual Rice_Production Rice_Production { get; set; }

        public int Rice_Produce_TotalBags { get; set; }

        public int Rice_Produce_TotalBagsProduce { get; set; }

        public int Rice_Produce_BagsSold { get; set; }

        public int Rice_Produce_RemainingBags { get; set; }

        public decimal Rice_Produce_Bag_ShortFall_total { get; set; }

        public decimal Rice_Produce_Bag_TotalExpenses { get; set; }

        public decimal Rice_Produce_Bag_TotalBPW { get; set; }

        public decimal Rice_Produce_Bag_TotalWorth { get; set; }

        public decimal Rice_Produce_Bag_TotalMarketWorth { get; set; }


        public decimal Rice_Produce_Bag_FactoryCost { get; set; }

        public decimal Rice_Produce_Bag_MarketFactoryCost { get; set; }

        public decimal Rice_Produce_Bag_StockWorth { get; set; }

        public decimal Rice_Produce_Bag_MarketStockWorth { get; set; }

        public decimal Rice_Produce_Bag_Average { get; set; }

        public decimal Rice_Produce_Bag_MarketAverage { get; set; }

        public decimal Rice_Produce_Bag_TotalWeight { get; set; }

        public decimal Rice_Produce_Bag_PerBagPrice { get; set; }

        public decimal Rice_Produce_Bag_TotalRawRate { get; set; }

        public decimal Rice_Produce_Bag_TotalMarketRawRate { get; set; }

        public decimal Rice_Produce_Bag_PerBagMarketPrice { get; set; }

        public DateTime Rice_Produce_Bag_Date { get; set; }

        public bool Status { get; set; }




    }
}