using DHMRice.Models;
using DHMRice.Models.HardCode;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DHMRice.Controllers
{
    [Authorize(Roles = "Shop,Admin")]
    public class ShopRiceSalesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProducedRiceSales
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GatePassOutward(int id)
        {
            var gate = db.GatePassOutward.Where(g => g.RiceTypeId == id && g.RiceType == "Shop Rice Sales").SingleOrDefault();
            if (gate == null)
            {
                var rawsale = db.ShopRiceSales_pt.Find(id);
                return View(rawsale);
            }
            else
            {
                return RedirectToAction("GatePass", "ShopRiceSales", new { id = id });
            }
        }
        [HttpPost]
        public ActionResult GatePassOutward(GatePassOutward GatePass, FormCollection form)
        {
            if (ModelState.IsValid)
            {

                GatePass.Date = DateTime.Now;
                GatePass.RiceType = "Shop Rice Sales";
                db.GatePassOutward.Add(GatePass);


                db.SaveChanges();
                var GatePassId = GatePass.RiceTypeId;
                return RedirectToAction("GatePass", "ShopRiceSales", new { id = GatePassId });
            }

            return View(GatePass);
        }
        public ActionResult GatePass(int id)
        {

            GatePassOutward Gatepas = db.GatePassOutward.Where(r => r.RiceTypeId == id && r.RiceType == "Shop Rice Sales").SingleOrDefault();
            db.SaveChanges();
            return View(Gatepas);
        }


        [HttpPost]
        public JsonResult Get_pt()
        {
            List<Tuple<ShopRiceSales_pt, string, decimal, decimal, decimal, int, int>> obj = new List<Tuple<ShopRiceSales_pt, string, decimal, decimal, decimal, int, int>>();
            var list = db.ShopRiceSales_pt.Where(m => m.srsp_status).ToList();
            foreach (var item in list)
            {
                try
                {
                    decimal recieved = db.Transaction_Shop.Where(m => m.status
                    &&
                    m.Transaction_Shop_item_id == item.srsp_id &&
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales ||
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales_Remaining
                    ).Sum(m => m.Credit);
                    int action = 0;
                    foreach (var oc in db.Opening_ClosingDays_Shop)
                    {
                        if (oc.Date.ToShortDateString() == item.srsp_date.ToShortDateString() && !oc.isClosed)
                        {
                            action = 1;
                            break;
                        }
                    }
                    int invoice_no = db.SaleInvoice.Where(m => m.Sale_id == item.srsp_id && m.SaleInvoice_type == SaleInvoiceType.ShopRiceSales).SingleOrDefault().SaleInvoice_no;

                    obj.Add(new Tuple<ShopRiceSales_pt, string, decimal, decimal, decimal, int, int>(item, item.srsp_date.ToShortDateString(), recieved, item.srsp_Total_Amount - recieved, item.srsp_Total_Amount - recieved, action, invoice_no));

                }
                catch (Exception)
                {

                }

            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ReturnStatus(int srsp_id)
        {
            int action = 0;
            try
            {
                var trans = db.Transaction_Shop.Where(
                         m => m.Transaction_Shop_item_id == srsp_id &&
                         m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales_Return
                         ).SingleOrDefault();
                action = (trans != null) ? 1 : 0;
            }
            catch (Exception ex)
            {
                action = 0;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCustomerRemainings(int Customer_Id)
        {
            var mShopSales = db.ShopRiceSales_pt.Where(m => m.Customer_Id == Customer_Id && m.srsp_status).ToList();
            var Transaction_Shops = new List<Transaction_Shop>();
            List<Tuple<ShopRiceSales_pt, decimal, decimal>> jsonret = new List<Tuple<ShopRiceSales_pt, decimal, decimal>>();
            foreach (var item in mShopSales)
            {
                try
                {
                    decimal Recieved = db.Transaction_Shop.Where(m => m.status
                    &&
                    m.Transaction_Shop_item_id == item.srsp_id &&
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales ||
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales_Remaining
                    ).Sum(m => m.Credit);
                    var trans = db.Transaction_Shop.Where(
                      m => m.Transaction_Shop_item_id == item.srsp_id &&
                      m.Transaction_Shop_item_type == SellingCategory.Produced_Rice_Sales_Return
                      ).SingleOrDefault();
                    Recieved = (trans != null) ? item.srsp_Total_Amount : Recieved;

                    decimal remaining = item.srsp_Total_Amount - Recieved;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<ShopRiceSales_pt, decimal, decimal>(item, Recieved, remaining));
                }
                catch (Exception)
                { }
            }
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCustomerRemainings1(int Customer_Id, int srsp_id)
        {

            var mShopSales = db.ShopRiceSales_pt.Where(m => m.Customer_Id == Customer_Id && m.srsp_status && m.srsp_id != srsp_id).ToList();
            var Transaction_Shops = new List<Transaction_Shop>();
            List<Tuple<ShopRiceSales_pt, decimal, decimal>> jsonret = new List<Tuple<ShopRiceSales_pt, decimal, decimal>>();
            foreach (var item in mShopSales)
            {
                try
                {
                    decimal Recieved = db.Transaction_Shop.Where(m => m.status
                    &&
                    m.Transaction_Shop_item_id == item.srsp_id &&
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales ||
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales_Remaining
                    ).Sum(m => m.Credit);
                    var trans = db.Transaction_Shop.Where(
                      m => m.Transaction_Shop_item_id == item.srsp_id &&
                      m.Transaction_Shop_item_type == SellingCategory.Produced_Rice_Sales_Return
                      ).SingleOrDefault();
                    Recieved = (trans != null) ? item.srsp_Total_Amount : Recieved;
                    decimal remaining = item.srsp_Total_Amount - Recieved;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<ShopRiceSales_pt, decimal, decimal>(item, Recieved, remaining));
                }
                catch (Exception)
                { }
            }
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_ch(int id)
        {
            var obj = db.ShopRiceSales_ch.Where(m => m.srsc_status && m.srsp_id == id).ToList();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_Customer(int srsp_id)
        {
            var obj = db.ShopRiceSales_pt.Find(srsp_id).Customer;
            return Json(obj, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult Get_Customer_Via_Code(FormCollection form)
        {
            try
            {
                var js = new JavaScriptSerializer();
                string Code = JsonConvert.DeserializeObject<string>(form["Code"]);
                if (Code != null)
                {
                    var obj = db.Customers.Where(m => (m.Status) && (m.Customer_Code == Code)).SingleOrDefault();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Get_Customer_Via_Mobile(FormCollection form)
        {
            try
            {
                var js = new JavaScriptSerializer();
                string Mobile = JsonConvert.DeserializeObject<string>(form["Mobile"]);
                if (Mobile != null)
                {
                    var obj = db.Customers.Where(m => (m.Status) && (m.Customer_MobileNo == Mobile)).SingleOrDefault();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Get_ShopRice()
        {
            var production_rice = db.ShopStock.Where(m => m.Status).ToList();
            return Json(production_rice, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ShopStockId(int id)
        {
            try
            {
                var mShopStock = db.ShopStock.Where(m => m.ShopStockId == id && m.ShopStock_pt.Shop_Id == 2).First();
                decimal srsc_ttl_qty = mShopStock.Qty;
                int srsc_sld_qty = mShopStock.SoldQty;
                int srsc_packing_type = mShopStock.packing_type;
                decimal srsc_Weight_kg_ttl = mShopStock.TotalWeight_KG;

                string srsc_title = mShopStock.Rice_Production.Rice_Production_name;
                decimal srsc_PerBagMarketPrice = mShopStock.PerBagPrice;
                Tuple<decimal, int, decimal, int, int, string, decimal> tpl = new Tuple<decimal, int, decimal, int, int, string, decimal>
                (
                srsc_ttl_qty,
                srsc_sld_qty,
                srsc_Weight_kg_ttl,
                srsc_packing_type,
                id,
                srsc_title,
                srsc_PerBagMarketPrice
                );

                return Json(tpl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult Return(int srsp_id)
        {
            ShopRiceSales_pt mShopRiceSales_pt = db.ShopRiceSales_pt.Where(m => m.srsp_id == srsp_id && m.srsp_status).SingleOrDefault();
            if (mShopRiceSales_pt != null)
            {
                foreach (var item in db.ShopRiceSales_ch.Where(m => m.srsp_id == mShopRiceSales_pt.srsp_id && m.srsc_status).ToList())
                {
                    //Again add in stock
                    var mstock = db.ShopStock.Find(item.ShopStockId);
                    mstock.SoldQty -= item.srsc_qty;
                    db.Entry(mstock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                var Receivedtrans = db.Transaction.Where(
                      m => m.status &&
                      m.Transaction_item_id == mShopRiceSales_pt.srsp_id &&
                          (m.Transaction_item_type == SellingCategory.RawRice_Sales || m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining)
                      ).SingleOrDefault();
                decimal recieved = (Receivedtrans != null) ? Receivedtrans.Credit : 0;

                Transaction_Shop trans = new Transaction_Shop();
                trans.isByCash = true;
                trans.BankAccountNo = "";
                foreach (var item in db.Opening_ClosingDays_Shop)
                {
                    if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                    {
                        trans.Opening_ClosingDays_Shop_id = item.Opening_ClosingDays_Shop_id;
                        break;
                    }
                }
                trans.Transaction_Shop_DateTime = DateTime.Now;
                trans.Transaction_Shop_Description = "RawRice Return from " + db.Customers.Find(mShopRiceSales_pt.Customer_Id).Customer_Name;
                trans.Transaction_Shop_item_id = srsp_id;
                trans.Transaction_Shop_item_type = SellingCategory.Shop_Rice_Sales_Return;
                trans.Debit = recieved;
                trans.Credit = 0;
                trans.status = (recieved > 0) ? true : false;
                db.Transaction_Shop.Add(trans);
                db.SaveChanges();
            }
            return Json(1, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public void Insert_sales(FormCollection form)
        {
            var js = new JavaScriptSerializer();
            decimal Recieved_Amount = Convert.ToDecimal(form["RecievedAmount"]);
            try
            {
                List<int> remaining_rsp_id = js.Deserialize<List<int>>(form["remaining_srsp_id"]);
                for (int i = 0; i < remaining_rsp_id.Count; i++)
                {
                    var mShopRiceSales_pt = db.ShopRiceSales_pt.Find(remaining_rsp_id[i]);
                    var rec = db.Transaction_Shop.Where(m => m.status
                    &&
                    m.Transaction_Shop_item_id == mShopRiceSales_pt.srsp_id &&
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales ||
                    m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales_Remaining
                    ).Sum(m => m.Credit);
                    var Remaining = mShopRiceSales_pt.srsp_Total_Amount - rec;

                    Transaction_Shop trans = new Transaction_Shop();
                    if (form["isBankAccount"] == "true")
                    {
                        trans.BankAccountNo = form["BankAccountNo"];
                    }
                    else if (form["isCheckbook"] == "true")
                    {
                        trans.checkno = Convert.ToInt32(form["CheckNo"]);
                        trans.BankAccountNo = form["BankAccountNo"];
                    }
                    else if (form["isCash"] == "true")
                    {
                        trans.isByCash = true;
                        trans.BankAccountNo = "";
                    }
                    foreach (var item in db.Opening_ClosingDays_Shop)
                    {
                        if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                        {
                            trans.Opening_ClosingDays_Shop_id = item.Opening_ClosingDays_Shop_id;
                            break;
                        }
                    }
                    trans.Transaction_Shop_DateTime = DateTime.Now;
                    trans.Transaction_Shop_Description = "Received Remaining from " + db.Customers.Find(mShopRiceSales_pt.Customer_Id).Customer_Name;
                    trans.Transaction_Shop_item_id = remaining_rsp_id[i];
                    trans.Transaction_Shop_item_type = SellingCategory.Shop_Rice_Sales_Remaining;
                    trans.Debit = 0;
                    trans.Credit = Remaining;
                    trans.status = true;
                    db.Transaction_Shop.Add(trans);
                    db.SaveChanges();
                    Recieved_Amount -= Remaining;
                }

            }
            catch (Exception ex)
            {

            }

            ShopRiceSales_pt ShopRiceSales_pt = js.Deserialize<ShopRiceSales_pt>(form["ShopRiceSales_pt"]);
            if (ShopRiceSales_pt.Customer.Customer_Id > 0)
            {
                ShopRiceSales_pt.Customer_Id = ShopRiceSales_pt.Customer.Customer_Id;
                ShopRiceSales_pt.Customer = null;
            }
            else
            {
                // string idd = Convert.ToString(Session["UserId"]);
                string idd = User.Identity.GetUserId();
                ShopRiceSales_pt.Customer.Id = idd;
                ShopRiceSales_pt.Customer.Status = true;
                db.Customers.Add(ShopRiceSales_pt.Customer);
                db.SaveChanges();

                ShopRiceSales_pt.Customer_Id = db.Customers.Max(m => m.Customer_Id);
                ShopRiceSales_pt.Customer = null;
            }
            ShopRiceSales_pt.srsp_Title = ShopRiceSales_pt.srsp_Title + " to Customer " + db.Customers.Find(ShopRiceSales_pt.Customer_Id).Customer_Name;
            ShopRiceSales_pt.srsp_status = true;
            ShopRiceSales_pt.srsp_date = DateTime.Now;
            ShopRiceSales_pt.Shop_Id = 2;
            db.ShopRiceSales_pt.Add(ShopRiceSales_pt);
            db.SaveChanges();
            int srsp_id = db.ShopRiceSales_pt.Max(m => m.srsp_id);
            List<ShopRiceSales_ch> ShopRiceSales_ch = js.Deserialize<ShopRiceSales_ch[]>(form["ShopRiceSales_ch"].ToString()).ToList();
            foreach (var item in ShopRiceSales_ch)
            {
                item.srsp_id = srsp_id;
                item.srsc_status = true;
                db.ShopRiceSales_ch.Add(item);
                db.SaveChanges();

                var mstock = db.ShopStock.Find(item.ShopStockId);
                mstock.SoldQty += item.srsc_qty;
                db.Entry(mstock).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();



            }
            Transaction_Shop trans_this = new Transaction_Shop();
            if (form["isBankAccount"] == "true")
            {
                trans_this.BankAccountNo = form["BankAccountNo"];
            }
            else if (form["isCheckbook"] == "true")
            {
                trans_this.checkno = Convert.ToInt32(form["CheckNo"]);
                trans_this.BankAccountNo = form["BankAccountNo"];
            }
            else if (form["isCash"] == "true")
            {
                trans_this.isByCash = true;
                trans_this.BankAccountNo = "";
            }
            foreach (var item in db.Opening_ClosingDays_Shop)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    trans_this.Opening_ClosingDays_Shop_id = item.Opening_ClosingDays_Shop_id;
                    break;
                }
            }
            trans_this.Transaction_Shop_DateTime = DateTime.Now;
            trans_this.Transaction_Shop_Description = ShopRiceSales_pt.srsp_Title;
            trans_this.Transaction_Shop_item_id = srsp_id;
            trans_this.Transaction_Shop_item_type = SellingCategory.Shop_Rice_Sales;
            trans_this.Debit = 0;
            trans_this.Credit = Recieved_Amount;
            trans_this.status = true;
            db.Transaction_Shop.Add(trans_this);
            db.SaveChanges();


            SaleInvoiceType mSaleInvoiceType = new SaleInvoiceType();
            SaleInvoice saleInvoice = new SaleInvoice()
            {
                SaleInvoice_no = mSaleInvoiceType.GenerateInvoiceNo(SaleInvoiceType.ShopRiceSales),
                SaleInvoice_type = SaleInvoiceType.ShopRiceSales,
                Sale_id = db.ShopRiceSales_pt.Max(m => m.srsp_id)
            };
            db.SaleInvoice.Add(saleInvoice);
            db.SaveChanges();
        }

        [HttpPost]
        public void Update_sales(FormCollection form)
        {


            var js = new JavaScriptSerializer();
            decimal Recieved_Amount = Convert.ToDecimal(form["RecievedAmount"]);
            try
            {
                List<int> remaining_srsp_id = js.Deserialize<List<int>>(form["remaining_srsp_id"]);
                for (int i = 0; i < remaining_srsp_id.Count; i++)
                {
                    var ShopRiceSales_pt1 = db.ShopRiceSales_pt.Find(remaining_srsp_id[i]);
                    var rec = db.Transaction_Shop.Where(m => m.status
                     &&
                     m.Transaction_Shop_item_id == ShopRiceSales_pt1.srsp_id &&
                     m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales ||
                     m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales_Remaining
                    ).Sum(m => m.Credit);
                    var Remaining = ShopRiceSales_pt1.srsp_Total_Amount - rec;

                    Transaction_Shop trans = new Transaction_Shop();
                    if (form["isBankAccount"] == "true")
                    {
                        trans.BankAccountNo = form["BankAccountNo"];
                    }
                    else if (form["isCheckbook"] == "true")
                    {
                        trans.checkno = Convert.ToInt32(form["CheckNo"]);
                        trans.BankAccountNo = form["BankAccountNo"];
                    }
                    else if (form["isCash"] == "true")
                    {
                        trans.isByCash = true;
                        trans.BankAccountNo = "";
                    }
                    foreach (var item in db.Opening_ClosingDays_Shop)
                    {
                        if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                        {
                            trans.Opening_ClosingDays_Shop_id = item.Opening_ClosingDays_Shop_id;
                            break;
                        }
                    }
                    trans.Transaction_Shop_DateTime = DateTime.Now;
                    trans.Transaction_Shop_Description = "Received Remaining from " + db.Customers.Find(ShopRiceSales_pt1.Customer_Id).Customer_Name;
                    trans.Transaction_Shop_item_id = remaining_srsp_id[i];
                    trans.Transaction_Shop_item_type = SellingCategory.Shop_Rice_Sales_Remaining;
                    trans.Debit = 0;
                    trans.Credit = Remaining;
                    trans.status = true;
                    db.Transaction_Shop.Add(trans);
                    db.SaveChanges();
                    Recieved_Amount -= Remaining;
                }

            }
            catch (Exception ex)
            {

            }

            ShopRiceSales_pt ShopRiceSales_pt_view = js.Deserialize<ShopRiceSales_pt>(form["ShopRiceSales_pt"]);
            var ShopRiceSales_pt = db.ShopRiceSales_pt.Find(ShopRiceSales_pt_view.srsp_id);
            if (ShopRiceSales_pt_view.Customer.Customer_Id > 0)
            {
                ShopRiceSales_pt.Customer_Id = ShopRiceSales_pt_view.Customer.Customer_Id;
                ShopRiceSales_pt.Customer = null;
            }
            else
            {
                // string idd = Convert.ToString(Session["UserId"]);
                string idd = User.Identity.GetUserId();
                ShopRiceSales_pt_view.Customer.Id = idd;
                ShopRiceSales_pt.Customer.Status = true;
                db.Customers.Add(ShopRiceSales_pt_view.Customer);
                db.SaveChanges();

                ShopRiceSales_pt.Customer_Id = db.Customers.Max(m => m.Customer_Id);
                ShopRiceSales_pt.Customer = null;
            }
            //ShopRiceSales_pt.Customer_Id = ShopRiceSales_pt_view.Customer_Id;
            ShopRiceSales_pt.srsp_TotalWeight_KG = ShopRiceSales_pt_view.srsp_TotalWeight_KG;
            ShopRiceSales_pt.srsp_TotalWeight_Mann = ShopRiceSales_pt_view.srsp_TotalWeight_Mann;
            ShopRiceSales_pt.srsp_Total_Amount = ShopRiceSales_pt_view.srsp_Total_Amount;
            ShopRiceSales_pt.srsp_Title = ShopRiceSales_pt_view.srsp_Title + " to Customer " + db.Customers.Find(ShopRiceSales_pt_view.Customer_Id).Customer_Name;
            ShopRiceSales_pt.srsp_status = true;
            ShopRiceSales_pt.srsp_date = DateTime.Now;
            ShopRiceSales_pt.Shop_Id = 2;
            db.Entry(ShopRiceSales_pt).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            int srsp_id = ShopRiceSales_pt.srsp_id;
            var obj = js.DeserializeObject(form["ShopRiceSales_ch"].ToString());
            List<ShopRiceSales_ch> ShopRiceSales_ch = js.Deserialize<ShopRiceSales_ch[]>(form["ShopRiceSales_ch"].ToString()).ToList();
            foreach (var item in ShopRiceSales_ch)
            {
                var rsc = db.ShopRiceSales_ch.Find(item.srsc_id);
                if (rsc == null)
                {
                    rsc = new ShopRiceSales_ch();
                }
                rsc.ShopStockId = item.ShopStockId;
                rsc.srsc_price = item.srsc_price;
                rsc.srsc_qty = item.srsc_qty;
                rsc.srsc_status = item.srsc_status;
                rsc.srsc_title = item.srsc_title;
                rsc.srsc_Weight_kg = item.srsc_Weight_kg;
                rsc.srsc_Weight_mann = item.srsc_Weight_mann;
                rsc.srsp_id = srsp_id;

                rsc.srsc_status = true;
                if (item.srsc_id == 0)
                {
                    db.ShopRiceSales_ch.Add(rsc);
                    db.SaveChanges();


                    var mstock = db.ShopStock.Find(item.ShopStockId);
                    mstock.SoldQty += item.srsc_qty;
                    db.Entry(mstock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {

                    int srsc_qty = 0;
                    using (var newdb = new ApplicationDbContext())
                    {
                        srsc_qty = newdb.ShopRiceSales_ch.Where(m => m.srsc_id == item.srsc_id).First().srsc_qty;
                    }
                    var mstock = db.ShopStock.Find(rsc.ShopStockId);
                    mstock.SoldQty -= srsc_qty;
                    db.Entry(mstock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    db.Entry(rsc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    mstock = db.ShopStock.Find(item.ShopStockId);
                    mstock.SoldQty += item.srsc_qty;
                    db.Entry(mstock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();





                }
            }

            List<ShopRiceSales_ch> Deleted_ShopRice_Sales_ch = js.Deserialize<ShopRiceSales_ch[]>(form["Deleted_ShopRiceSales_ch"].ToString()).ToList();
            foreach (var item in Deleted_ShopRice_Sales_ch)
            {
                var mstock = db.ShopStock.Find(item.ShopStockId);
                mstock.SoldQty -= item.srsc_qty;
                db.Entry(mstock).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var srsc = db.ShopRiceSales_ch.Find(item.srsc_id);
                srsc.srsc_status = false;
                db.Entry(srsc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            foreach (var item in db.Transaction_Shop.Where(m => m.Transaction_Shop_item_id == ShopRiceSales_pt.srsp_id && m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales && m.status))
            {
                db.Transaction_Shop.Remove(item);
            }
            db.SaveChanges();
            Transaction_Shop trans_this = new Transaction_Shop();
            if (form["isBankAccount"] == "true")
            {
                trans_this.BankAccountNo = form["BankAccountNo"];
            }
            else if (form["isCheckbook"] == "true")
            {
                trans_this.checkno = Convert.ToInt32(form["CheckNo"]);
                trans_this.BankAccountNo = form["BankAccountNo"];
            }
            else if (form["isCash"] == "true")
            {
                trans_this.isByCash = true;
                trans_this.BankAccountNo = "";
            }
            foreach (var item in db.Opening_ClosingDays_Shop)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    trans_this.Opening_ClosingDays_Shop_id = item.Opening_ClosingDays_Shop_id;
                    break;
                }
            }
            trans_this.Transaction_Shop_DateTime = DateTime.Now;
            trans_this.Transaction_Shop_Description = ShopRiceSales_pt.srsp_Title;
            trans_this.Transaction_Shop_item_id = srsp_id;
            trans_this.Transaction_Shop_item_type = SellingCategory.Shop_Rice_Sales;
            trans_this.Debit = 0;
            trans_this.Credit = Recieved_Amount;
            trans_this.status = true;
            db.Transaction_Shop.Add(trans_this);
            db.SaveChanges();
        }



        [HttpPost]
        public void Delete_sales(int id)
        {
            var ShopRiceSales_pt = db.ShopRiceSales_pt.Find(id);
            if (ShopRiceSales_pt != null)
            {
                var ShopRiceSales_ch = db.ShopRiceSales_ch.Where(m => m.srsp_id == ShopRiceSales_pt.srsp_id).ToList();
                foreach (var item in ShopRiceSales_ch)
                {
                    var mstock = db.ShopStock.Find(item.ShopStockId);
                    mstock.SoldQty -= item.srsc_qty;
                    db.Entry(mstock).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var srsc = db.ShopRiceSales_ch.Find(item.srsc_id);
                    srsc.srsc_status = false;
                    db.Entry(srsc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                }
                ShopRiceSales_pt.srsp_status = false;
                db.Entry(ShopRiceSales_pt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                foreach (var item in db.Transaction_Shop.Where(m => m.Transaction_Shop_item_id == ShopRiceSales_pt.srsp_id && m.Transaction_Shop_item_type == SellingCategory.Shop_Rice_Sales && m.status))
                {
                    db.Transaction_Shop.Remove(item);
                }
                db.SaveChanges();
            }

        }
        [HttpPost]
        public void ReceivedRemaining(FormCollection form)
        {
            var ShopRiceSales_pt = db.ShopRiceSales_pt.Find(Convert.ToInt32(form["id"]));
            // var rec = db.Transaction_Shop_Shop.Where(m => m.Transaction_Shop_Shop_item_id == ShopRiceSales_pt.srsp_id && m.Transaction_Shop_Shop_item_type == "Shop Rice Sales").Sum(m => m.Credit);
            // var Remaining = ShopRiceSales_pt.srsp_Total_Amount - rec;

            Transaction_Shop trans = new Transaction_Shop();

            trans.isByCash = true;
            trans.BankAccountNo = "";

            foreach (var item in db.Opening_ClosingDays_Shop)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    trans.Opening_ClosingDays_Shop_id = item.Opening_ClosingDays_Shop_id;
                    break;
                }
            }
            trans.Transaction_Shop_DateTime = DateTime.Now;
            trans.Transaction_Shop_Description = "Received Remaining from " + db.Customers.Find(ShopRiceSales_pt.Customer_Id).Customer_Name;
            trans.Transaction_Shop_item_id = Convert.ToInt32(form["id"]);
            trans.Transaction_Shop_item_type = SellingCategory.Shop_Rice_Sales;
            trans.Debit = 0;
            trans.Credit = Convert.ToInt32(form["Remaining"]);
            trans.status = true;
            db.Transaction_Shop.Add(trans);
            db.SaveChanges();
        }
    }
}