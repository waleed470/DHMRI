using DHMRice.Models;
using DHMRice.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class RiceProductionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RiceProduction
        public ActionResult Index()
        {
            return View(db.Rice_Productions.Where(m => m.Status).ToList());
        }

        public ActionResult WorthRice()
        {
            return View(db.Rice_Production_ProductWorths.ToList());
        }
        public ActionResult PreviousRice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PreviousRice(FormCollection form, Rice_Production Rice, Rice_Produce_Bag Rice_Produce_Bag)
        {
            string idd = Convert.ToString(Session["UserId"]);
                        
            var RiceName = Convert.ToString(form["RiceName"]);

            Rice.Rice_Production_name = RiceName;
            Rice.Rice_Production_Code = Convert.ToString(form["RiceCode"]);
            Rice.Packing_Id = Convert.ToInt32(form["packing"]);
            Rice.Rice_Production_Date = DateTime.Now;
            Rice.Id = idd;
            Rice.Status = true;
            db.Rice_Productions.Add(Rice);
            db.SaveChanges();
            var Rice_Production_id = db.Rice_Productions.Max(m => m.Rice_Production_id);

            Rice_Produce_Bag.Rice_Production_id = Rice_Production_id;
            Rice_Produce_Bag.Rice_Produce_TotalBagsProduce = Convert.ToInt32(form["BAgs"]);
            Rice_Produce_Bag.Rice_Produce_Bag_PerBagMarketPrice = Convert.ToDecimal(form["Price"]);
            Rice_Produce_Bag.Status = true;
            Rice_Produce_Bag.Rice_Produce_Bag_Date = DateTime.Now;
            db.Rice_Produce_Bags.Add(Rice_Produce_Bag);
            db.SaveChanges();

            return RedirectToAction("Index");
            }


        public ActionResult RiceStockEdit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rice_Production RawRice = db.Rice_Productions.Find(id);

            if (RawRice == null)
            {
                return HttpNotFound();
            }
           
            return View(RawRice);
        }
        [HttpPost]
        public ActionResult RiceStockEdit(FormCollection form, Rice_Production Ricee, Rice_Produce_Bag Rice_Produce_Bag)
        {
            string idd = Convert.ToString(Session["UserId"]);

            int Rice_Production_id = Convert.ToInt32(form["Rice_Production_id"]);
            var RiceName = Convert.ToString(form["RiceName"]);
            Rice_Production rice = db.Rice_Productions.Find(Rice_Production_id);

            rice.Rice_Production_name = RiceName;
            rice.Rice_Production_Code = Convert.ToString(form["RiceCode"]);
            rice.Packing_Id = Convert.ToInt32(form["packing"]);
            rice.Rice_Production_Date = DateTime.Now;
            rice.Id = idd;
            rice.Status = true;

            db.Entry(rice).State = EntityState.Modified;
            db.SaveChanges();

            var price = db.Rice_Produce_Bags.Where(p => p.Rice_Production_id == Rice_Production_id).SingleOrDefault();

            price.Rice_Production_id = Rice_Production_id;
            price.Rice_Produce_TotalBagsProduce = Convert.ToInt32(form["BAgs"]);
            price.Rice_Produce_Bag_PerBagMarketPrice = Convert.ToDecimal(form["Price"]);
            price.Status = true;
            price.Rice_Produce_Bag_Date = DateTime.Now;
    
            db.Entry(price).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult WorthRiceStock()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WorthRiceStock(FormCollection form,Rice_Production_ProductWorth worth)
        {
            string idd = Convert.ToString(Session["UserId"]);

            var RiceName = Convert.ToString(form["RiceName"]);

            worth.Rice_Production_ProductWorth_name = RiceName;

            worth. Rice_Production_ProductWorth_Qty = Convert.ToInt32(form["BAgs"]);
            worth.Rice_Production_ProductWorth_PBA = Convert.ToDecimal(form["Price"]);
            db.Rice_Production_ProductWorths.Add(worth);
            db.SaveChanges();
         
            return RedirectToAction("Index");
        }

        public ActionResult Index2()
        {
            return View(db.Rice_Productions.Where(m => m.Status).ToList());
        }
        public ActionResult AddNew()
        {

            ViewBag.Party_Id = new SelectList(db.Parties.Where(m => m.Status == true).ToList(), "Party_Id", "Party_Name");

            ViewBag.RawRice_id = new SelectList(db.RarRices.Where(m => m.Status == true).ToList(), "RawRice_id", "Item_Name");

            var Pricing = db.Pricing.Where(m => m.Status == true).ToList();

            var RawRice = db.RarRices.Where(m => m.Status == true).ToList();

            ViewBag.Rice_category_Id = new SelectList(db.Rice_Categories.Where(m => m.Status == true).ToList(), "Rice_category_Id", "Rice_Category_Name");

            ViewBag.Packing_Id = new SelectList(db.Packings.ToList(), "Packing_Id", "Packing_Type");

            var viewModel = new ProductionViewModel
            {
                RawRices = RawRice,
                Pricings = Pricing

            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddNew(Rice_Production Rice_Production,Rice_Produce_Bag Rice_Produce_Bag,   List<Rice_Production_ShortFall> Rice_Production_ShortFall, List<Production_Rice> Production_Rice, List<Rice_Production_Expense> Rice_Production_Expense, List<Rice_Production_ProductWorth> Rice_Production_ProductWorth,Pricing pricing,Production_Extra_Rice Extra_Rice ,FormCollection form)
            {


            string idd = Convert.ToString(Session["UserId"]);
            Rice_Production.Id = idd;
            Rice_Production.Rice_Production_name = Convert.ToString(form["Rice_Production_name"]);
            Rice_Production.Rice_Production_Code = Convert.ToString(form["Rice_Production_Code"]);
            Rice_Production.Packing_Id = Convert.ToInt32(form["Packing_Id"]);
            Rice_Production.Rice_Production_Date = DateTime.Now;
            Rice_Production.Status = true;
            db.Rice_Productions.Add(Rice_Production);
            db.SaveChanges();

            
            var Rice_Production_id = db.Rice_Productions.Max(m => m.Rice_Production_id);

            Rice_Produce_Bag.Rice_Production_id = Rice_Production_id;
            Rice_Produce_Bag.Rice_Produce_Bag_TotalRawRate = Convert.ToDecimal(form["Rice_Production_TotalRate"]);
            Rice_Produce_Bag.Rice_Produce_TotalBags = Convert.ToInt32(form["Rice_Production_TotalBags"]);
            Rice_Produce_Bag.Rice_Produce_TotalBagsProduce = Convert.ToInt32(form["Rice_Production_Bags"]);
            Rice_Produce_Bag.Rice_Produce_Bag_TotalWeight = Convert.ToDecimal(form["Rice_Production_TotalWieght"]);
            Rice_Produce_Bag.Rice_Produce_Bag_Average = Convert.ToDecimal(form["Rice_Production_Average"]);
            Rice_Produce_Bag.Rice_Produce_Bag_TotalWorth = Convert.ToDecimal(form["Rice_Production_TotalWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_ShortFall_total = Convert.ToDecimal(form["Rice_Production_ShortFall_total"]);
            Rice_Produce_Bag.Rice_Produce_Bag_TotalExpenses = Convert.ToDecimal(form["Rice_Production_TotalExpense"]);
            Rice_Produce_Bag.Rice_Produce_Bag_StockWorth = Convert.ToDecimal(form["Rice_Production_StockWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_TotalBPW = Convert.ToDecimal(form["Rice_Production_TotalByProductWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_FactoryCost = Convert.ToDecimal(form["FactoryCost"]);
            Rice_Produce_Bag.Rice_Produce_Bag_MarketAverage = Convert.ToDecimal(form["Rice_Produce_Bag_MarketAverage"]);
            Rice_Produce_Bag.Rice_Produce_Bag_MarketStockWorth = Convert.ToDecimal(form["Rice_Produce_Bag_MarketStockWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_MarketFactoryCost = Convert.ToDecimal(form["Rice_Produce_Bag_MarketFactoryCost"]);

            Rice_Produce_Bag.Rice_Produce_Bag_PerBagPrice = Convert.ToDecimal(form["Rice_Production_perBagPrice"]);
            Rice_Produce_Bag.Rice_Produce_Bag_PerBagMarketPrice = Convert.ToDecimal(form["MNewPerBagPrice"]);
            Rice_Produce_Bag.Status = true;
            Rice_Produce_Bag.Rice_Produce_Bag_Date = DateTime.Now;

            db.Rice_Produce_Bags.Add(Rice_Produce_Bag);
            db.SaveChanges();
            var Rice_Produce_Bags_id = db.Rice_Produce_Bags.Max(m => m.Rice_Produce_Bags_id);

            //pricing.item_id = Rice_Production_id;
            //pricing.item_Type = "Rice Production";
            //pricing.PerBagMarketPrice = Convert.ToDecimal(form["MNewPerBagPrice"]);
            //pricing.PerBagPrice = Convert.ToDecimal(form["Rice_Production_perBagPrice"]);
            //pricing.Pricing_Date = DateTime.Now;
            //pricing.Status = true;
            //db.Pricing.Add(pricing);
            //db.SaveChanges();

            Extra_Rice.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
            Extra_Rice.Extra_Rice = Convert.ToDecimal(form["ExtraRice"]);
            db.Production_Extra_Rice.Add(Extra_Rice);
            db.SaveChanges();


            foreach (var item in Production_Rice)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Production_Rices.Add(item);
                db.SaveChanges();

                var rawrice = db.RarRices.Find(item.RawRice_id);
                rawrice.Bags_Sold_qty += item.Purchase_Rice_BagsUsed;
                db.Entry(rawrice).State = EntityState.Modified;
                db.SaveChanges();
            }

            foreach (var item in Rice_Production_Expense)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Rice_Production_Expenses.Add(item);
                db.SaveChanges();
            }

            foreach (var item in Rice_Production_ProductWorth)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Rice_Production_ProductWorths.Add(item);
                db.SaveChanges();
            }

            foreach (var item in Rice_Production_ShortFall)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Rice_Production_ShortFalls.Add(item);
                db.SaveChanges();
            }
            
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public JsonResult GetRice()
        {
            var list = db.RarRices.Where(c => c.Status == true).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IncreaseRicee(int? id)
        {

            ViewBag.Party_Id = new SelectList(db.Parties.Where(m => m.Status == true).ToList(), "Party_Id", "Party_Name");

            ViewBag.RawRice_id = new SelectList(db.RarRices.Where(m => m.Status == true).ToList(), "RawRice_id", "Item_Name");

            var ProductionRice = db.Rice_Productions.Find(id);
            //var Pricing = db.Pricing.Where(p=> p.item_id==ProductionRice.Rice_Production_id && p.item_Type== "Rice Production").SingleOrDefault();


            //var RawRice = db.RarRices.Where(m => m.Status == true).ToList();

            ViewBag.Rice_category_Id = new SelectList(db.Rice_Categories.Where(m => m.Status == true).ToList(), "Rice_category_Id", "Rice_Category_Name");

            ViewBag.Packing_Id = new SelectList(db.Packings.ToList(), "Packing_Id", "Packing_Type");

            //var viewModel = new ProductionEditViewModel
            //{
            //   Rice_Productions = ProductionRice,
            //   Pricings = Pricing,
            //   RawRices = RawRice

            //};
            return View(ProductionRice);
        }
        [HttpPost]
        public ActionResult IncreaseRicee(Rice_Production Rice_Production, Rice_Produce_Bag Rice_Produce_Bag, List<Rice_Production_ShortFall> Rice_Production_ShortFall, List<Production_Rice> Production_Rice, List<Rice_Production_Expense> Rice_Production_Expense, List<Rice_Production_ProductWorth> Rice_Production_ProductWorth, Pricing pricing, Production_Extra_Rice Extra_Rice, FormCollection form)
        {



            int Rice_Production_id = Convert.ToInt32(form["Rice_Production_id"]);
            var Rice_Production_Ream = db.Rice_Productions.Find(Rice_Production_id);
            int Remaining = Rice_Production_Ream.Rice_Production_RemainingBags;





            Rice_Produce_Bag.Rice_Production_id = Rice_Production_id;
            Rice_Produce_Bag.Rice_Produce_Bag_TotalRawRate = Convert.ToDecimal(form["Rice_Production_TotalRate"]);
            Rice_Produce_Bag.Rice_Produce_TotalBags = Convert.ToInt32(form["Rice_Production_TotalBags"]);
            Rice_Produce_Bag.Rice_Produce_TotalBagsProduce = Convert.ToInt32(form["Rice_Production_Bags"]);
            Rice_Produce_Bag.Rice_Produce_RemainingBags = Rice_Produce_Bag.Rice_Produce_TotalBagsProduce;
            Rice_Produce_Bag.Rice_Produce_Bag_TotalWeight = Convert.ToDecimal(form["Rice_Production_TotalWieght"]);
            Rice_Produce_Bag.Rice_Produce_Bag_Average = Convert.ToDecimal(form["Rice_Production_Average"]);
            Rice_Produce_Bag.Rice_Produce_Bag_TotalWorth = Convert.ToDecimal(form["Rice_Production_TotalWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_ShortFall_total = Convert.ToDecimal(form["Rice_Production_ShortFall_total"]);
            Rice_Produce_Bag.Rice_Produce_Bag_TotalExpenses = Convert.ToDecimal(form["Rice_Production_TotalExpense"]);
            Rice_Produce_Bag.Rice_Produce_Bag_StockWorth = Convert.ToDecimal(form["Rice_Production_StockWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_TotalBPW = Convert.ToDecimal(form["Rice_Production_TotalByProductWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_FactoryCost = Convert.ToDecimal(form["FactoryCost"]);
            Rice_Produce_Bag.Rice_Produce_Bag_MarketAverage = Convert.ToDecimal(form["Rice_Produce_Bag_MarketAverage"]);
            Rice_Produce_Bag.Rice_Produce_Bag_MarketStockWorth = Convert.ToDecimal(form["Rice_Produce_Bag_MarketStockWorth"]);
            Rice_Produce_Bag.Rice_Produce_Bag_MarketFactoryCost = Convert.ToDecimal(form["Rice_Produce_Bag_MarketFactoryCost"]);
            Rice_Produce_Bag.Rice_Produce_Bag_PerBagPrice = Convert.ToDecimal(form["Rice_Production_perBagPrice"]);
            Rice_Produce_Bag.Rice_Produce_Bag_PerBagMarketPrice = Convert.ToDecimal(form["MNewPerBagPrice"]);
            Rice_Produce_Bag.Status = true;
            Rice_Produce_Bag.Rice_Produce_Bag_Date = DateTime.Now;

            db.Rice_Produce_Bags.Add(Rice_Produce_Bag);
            db.SaveChanges();

            int TotalBags = Remaining + Rice_Produce_Bag.Rice_Produce_TotalBagsProduce;
            Rice_Production_Ream.Rice_Production_RemainingBags = TotalBags;
            db.Entry(Rice_Production_Ream).State = EntityState.Modified;
            db.SaveChanges();


            var Rice_Produce_Bags_id = db.Rice_Produce_Bags.Max(m => m.Rice_Produce_Bags_id);

            //pricing.item_id = Rice_Production_id;
            //pricing.item_Type = "Rice Production";
            //pricing.PerBagMarketPrice = Convert.ToDecimal(form["MNewPerBagPrice"]);
            //pricing.PerBagPrice = Convert.ToDecimal(form["Rice_Production_perBagPrice"]);
            //pricing.Pricing_Date = DateTime.Now;
            //pricing.Status = true;
            //db.Pricing.Add(pricing);
            //db.SaveChanges();

            Extra_Rice.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
            Extra_Rice.Extra_Rice = Convert.ToDecimal(form["ExtraRice"]);
            db.Production_Extra_Rice.Add(Extra_Rice);
            db.SaveChanges();


            foreach (var item in Production_Rice)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Production_Rices.Add(item);
                db.SaveChanges();
                var rawrice = db.RarRices.Find(item.RawRice_id);
                rawrice.Bags_Sold_qty += item.Purchase_Rice_BagsUsed;
                db.Entry(rawrice).State = EntityState.Modified;
                db.SaveChanges();

            }

            foreach (var item in Rice_Production_Expense)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Rice_Production_Expenses.Add(item);
                db.SaveChanges();
            }

            foreach (var item in Rice_Production_ProductWorth)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Rice_Production_ProductWorths.Add(item);
                db.SaveChanges();
            }

            foreach (var item in Rice_Production_ShortFall)
            {
                item.Rice_Produce_Bags_id = Rice_Produce_Bags_id;
                db.Rice_Production_ShortFalls.Add(item);
                db.SaveChanges();
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            var RicePRo = db.Rice_Productions.Find(id);
            var producebagsid = db.Rice_Produce_Bags.Where(r => r.Rice_Production_id == RicePRo.Rice_Production_id).Max(m=>m.Rice_Produce_Bags_id);


            if (RicePRo != null)
            {
                ViewBag.RawRice_id = new SelectList(db.RarRices.Where(m => m.Status == true).ToList(), "RawRice_id", "Item_Name", producebagsid);

                var Pricing = db.Pricing.Where(m => m.Status == true).ToList();

                var RawRice = db.RarRices.Where(m => m.Status == true).ToList();

                //ViewBag.Rice_category_Id = new SelectList(db.Rice_Categories.Where(m => m.Status == true).ToList(), "Rice_category_Id", "Rice_Category_Name", RawRice.Rice_category_Id);

                ViewBag.Packing_Id = new SelectList(db.Packings.ToList(), "Packing_Id", "Packing_Type",RicePRo.Packing_Id);




                return View(RicePRo);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public JsonResult GetProductData(string RawRice_id)
        {
            if(RawRice_id == "null")
            {
                return Json(new { success = false });

            }
            int riceid = Convert.ToInt32(RawRice_id);
            RawRice product = db.RarRices.Find(riceid);
            Packing packing = db.Packings.Find(product.Packing_Id);
            var rate = db.Pricing.Where(m => m.item_id == riceid && m.item_Type== "RawRice").SingleOrDefault();
            var remaining = product.Bags_qty;
            if (product.Bags_Sold_qty >0)
            {
                 remaining = product.Bags_qty - product.Bags_Sold_qty;
            }
            
            if (product != null)
            {
                return Json(new { success = true, Bags_qty = remaining, Packing_Id = product.Pb_Weight, PerBagPrice = rate.PerBagPrice, PerBagMarketPrice=rate.PerBagMarketPrice });
            }
            
            return Json(new { success = false });

        }
    }
}