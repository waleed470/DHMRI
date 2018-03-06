using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class ProductWorthSalesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductWorthSales
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Get_Worth_Rice_id(int id)
        {
            try
            {
                var riceBags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == id).Max(m=> m.Rice_Produce_Bags_id);
                var worthRice = db.Rice_Production_ProductWorths.Where(m => m.Rice_Produce_Bags_id == riceBags).ToList();



                return Json(worthRice, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult Get_Produced_Rice_id(int id)
        {
            try
            {
                decimal prsc_ttl_qty = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == id).Sum(m => m.Rice_Produce_TotalBagsProduce);
                int prsc_sld_qty = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == id).Sum(m => m.Rice_Produce_BagsSold);
                int prsc_packing_type = db.Rice_Productions.Find(id).packing.Packing_Type;
                int max_Bags_id = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == id).Max(m => m.Rice_Produce_Bags_id);
                decimal prsc_Weight_kg_ttl = db.Rice_Produce_Bags.OrderByDescending(p => p.Rice_Produce_Bag_Date).FirstOrDefault().Rice_Produce_Bag_TotalWeight;

                int Rice_Production_id = id;
                string prsc_title = db.Rice_Productions.Find(id).Rice_Production_name;
                decimal prsc_PerBagMarketPrice = db.Rice_Produce_Bags.OrderByDescending(p => p.Rice_Produce_Bag_Date).FirstOrDefault().Rice_Produce_Bag_PerBagMarketPrice;
                Tuple<decimal, int, decimal, int, int, string, decimal> tpl = new Tuple<decimal, int, decimal, int, int, string, decimal>
                (
                prsc_ttl_qty,
                prsc_sld_qty,
                prsc_Weight_kg_ttl,
                prsc_packing_type,
                Rice_Production_id,
                prsc_title,
                prsc_PerBagMarketPrice
                );

                return Json(tpl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
    }
}