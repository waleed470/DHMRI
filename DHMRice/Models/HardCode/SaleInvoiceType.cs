using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DHMRice.Models.HardCode
{
    public class SaleInvoiceType
    {

        public const string FactoryRiceSales = "FactoryRiceSales";

        public const string ShopRiceSales = "ShopRiceSales";

        private ApplicationDbContext db = new ApplicationDbContext();
        public int GenerateInvoiceNo(string Type)
        {

            int invoiceno = 0;
            try
            {
                invoiceno=db.SaleInvoice.Where(m=>m.SaleInvoice_type==Type).Max(m => m.SaleInvoice_no);
            }
            catch (Exception)
            {

            }
            return (invoiceno == 0) ? 1 : ++invoiceno;
        }
    }
}