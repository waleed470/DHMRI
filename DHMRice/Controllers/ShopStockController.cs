using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DHMRice.Controllers
{
    public class ShopStockController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShopStock
        public ActionResult Index()
        {
            return View();
        }

        public IQueryable<Shop> GetDepartment()
        {
            return db.Shopes;
        }


        public JsonResult Get_Shop()
        {
            var obj = db.Shopes.Where(m => m.Status).ToList();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_Stock()
        {
            //var datee = "";
            var obj = db.ShopStock.Where(m => m.Status).ToList();
            //foreach (var item in obj)
            //{
            //    datee = item.ShopDate.ToShortDateString();
            //}
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_Produced_Rice()
        {
            var production_rice = db.Rice_Productions.Where(m => m.Status).ToList();
            return Json(production_rice, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ch(int id)
        {
            var obj = db.ShopStock.Where(m => m.ShopStockId == id).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
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

        public void Insert_sales(FormCollection form)
        {
            string idd = Convert.ToString(Session["UserId"]);
            var js = new JavaScriptSerializer();
            decimal SumWeight_Mann = Convert.ToDecimal(form["SumWeight_Mann"]);
            decimal SumWeight_KG = Convert.ToDecimal(form["SumWeight_KG"]);
            decimal NetTotal = Convert.ToDecimal(form["NetTotal"]);
            int shopid = Convert.ToInt16(form["Shop_Id"]);

            List<ShopStock> ShopStock = js.Deserialize<ShopStock[]>(form["ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in ShopStock)
            {
                //item.ShopDate = DateTime.Now;
                item.NetTotal = NetTotal;
                item.SumWeight_Mann = SumWeight_Mann;
                item.SumWeight_KG = SumWeight_KG;
                item.Shop_Id = shopid;
                item.Id = idd;
                item.Status = true;

                db.ShopStock.Add(item);
                db.SaveChanges();

                var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).ToList();

                decimal qty = item.Qty;

                foreach (var obj in Rice_Produce_Bags)
                {
                    decimal diff = obj.Rice_Produce_TotalBagsProduce - obj.Rice_Produce_BagsSold;
                    if (diff > qty && qty > 0)
                    {
                        obj.Rice_Produce_BagsSold += Convert.ToInt32(qty);
                        obj.Rice_Produce_RemainingBags = obj.Rice_Produce_TotalBagsProduce - obj.Rice_Produce_BagsSold;
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        qty = 0;
                    }
                    else if (diff <= qty && qty > 0 && diff != 0)
                    {
                        obj.Rice_Produce_BagsSold += Convert.ToInt32(diff);
                        obj.Rice_Produce_RemainingBags = obj.Rice_Produce_TotalBagsProduce - obj.Rice_Produce_BagsSold;
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        qty -= diff;
                    }
                }

            }

        }


        [HttpPost]
        public void Update_sales(FormCollection form)
        {
            var js = new JavaScriptSerializer();


            int shopid = Convert.ToInt16(form["ShopStockId"]);
            var obj = js.DeserializeObject(form["ProducedRiceSales_ch"].ToString());
            List<ShopStock> ProducedRiceSales_ch = js.Deserialize<ShopStock[]>(form["ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in ProducedRiceSales_ch)
            {
                var rsc = db.ShopStock.Find(item.ShopStockId);
                if (rsc == null)
                {
                    rsc = new ShopStock();
                }
                rsc.Rice_Production_id = item.Rice_Production_id;
                rsc.PerBagPrice = item.PerBagPrice;
                rsc.Qty = item.Qty;
                rsc.TotalWeight_KG = item.TotalWeight_KG;
                rsc.TotalWeight_Mann = item.TotalWeight_Mann;
                rsc.SumWeight_KG = item.SumWeight_KG;
                rsc.SumWeight_Mann = item.SumWeight_Mann;
                rsc.Total_Amount = item.Total_Amount;
                rsc.packing_type = item.packing_type;
                rsc.NetTotal = item.NetTotal;
                rsc.PerBagPrice = item.packing_type;


                rsc.Status = true;
                if (item.ShopStockId == 0)
                {
                    db.ShopStock.Add(rsc);
                    db.SaveChanges();


                    var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).ToList();

                    decimal qty = item.Qty;

                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        decimal diff = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                        if (diff > qty && qty > 0)
                        {
                            obj1.Rice_Produce_BagsSold += Convert.ToInt32(qty);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            qty = 0;
                        }
                        else if (diff <= qty && qty > 0 && diff != 0)
                        {
                            obj1.Rice_Produce_BagsSold += Convert.ToInt32(diff);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            qty -= diff;
                        }

                    }
                }
                else
                {

                    int prsc_qty = 0;
                    using (var newdb = new ApplicationDbContext())
                    {
                        prsc_qty = newdb.ShopStock.Where(m => m.ShopStockId == item.ShopStockId).First().Qty;
                    }
                    var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).OrderByDescending(m => m.Rice_Produce_Bag_Date).ToList();

                    foreach (var obj_new in Rice_Produce_Bags)
                    {
                        if (prsc_qty >= obj_new.Rice_Produce_BagsSold && prsc_qty > 0)
                        {

                            decimal diff1 = prsc_qty - obj_new.Rice_Produce_BagsSold;
                            obj_new.Rice_Produce_BagsSold -= Convert.ToInt32(obj_new.Rice_Produce_BagsSold);
                            obj_new.Rice_Produce_RemainingBags = obj_new.Rice_Produce_TotalBagsProduce - obj_new.Rice_Produce_BagsSold;
                            db.Entry(obj_new).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            prsc_qty = (int)diff1;
                        }
                        else if (prsc_qty < obj_new.Rice_Produce_BagsSold && prsc_qty > 0)
                        {
                            decimal diff1 = prsc_qty - obj_new.Rice_Produce_BagsSold;
                            obj_new.Rice_Produce_BagsSold -= Convert.ToInt32(prsc_qty);
                            obj_new.Rice_Produce_RemainingBags = obj_new.Rice_Produce_TotalBagsProduce - obj_new.Rice_Produce_BagsSold;
                            db.Entry(obj_new).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            prsc_qty = (int)diff1;
                        }
                    }
                    db.Entry(rsc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();




                    Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).ToList();

                    decimal qty = item.Qty;
                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        decimal diff = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                        if (diff > qty && qty > 0)
                        {
                            obj1.Rice_Produce_BagsSold += Convert.ToInt32(qty);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            qty = 0;
                        }
                        else if (diff <= qty && qty > 0 && diff != 0)
                        {
                            obj1.Rice_Produce_BagsSold += Convert.ToInt32(diff);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            qty -= diff;
                        }

                    }
                }

            }

        }

        [HttpPost]
        public void Delete_sales(int id)
        {
            var ProducedRiceSales_pt = db.ShopStock.Find(id);
            if (ProducedRiceSales_pt != null)
            {
                var ProducedRiceSales_ch = db.ShopStock.Where(m => m.ShopStockId == ProducedRiceSales_pt.ShopStockId).ToList();
                foreach (var item in ProducedRiceSales_ch)
                {
                    var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).OrderByDescending(m => m.Rice_Produce_Bag_Date).ToList();

                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        if (item.Qty >= obj1.Rice_Produce_BagsSold && item.Qty > 0)
                        {

                            decimal diff1 = item.Qty - obj1.Rice_Produce_BagsSold;
                            obj1.Rice_Produce_BagsSold -= Convert.ToInt32(obj1.Rice_Produce_BagsSold);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            item.Qty = (int)diff1;
                        }
                        else if (item.Qty < obj1.Rice_Produce_BagsSold && item.Qty > 0)
                        {
                            decimal diff1 = item.Qty - obj1.Rice_Produce_BagsSold;
                            obj1.Rice_Produce_BagsSold -= Convert.ToInt32(item.Qty);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            item.Qty = (int)diff1;
                        }

                    }

                    item.Status = false;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                ProducedRiceSales_pt.Status = false;
                db.Entry(ProducedRiceSales_pt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            
            }

        }
    }
}