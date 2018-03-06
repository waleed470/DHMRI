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
using System.Web.Security;

namespace DHMRice.Controllers
{
    [Authorize(Roles = "Factory,Admin")]
    public class ProducedRiceSalesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProducedRiceSales
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Get_pt()
        {
            List<Tuple<ProducedRiceSales_pt, string, decimal, decimal,decimal, int,int>> obj = new List<Tuple<ProducedRiceSales_pt, string, decimal, decimal, decimal, int,int>>();
            var list = db.ProducedRiceSales_pt.Where(m => m.prsp_status).ToList();
            foreach (var item in list)
            {
                try
                {
                     decimal recieved = db.Transaction.Where(m => m.status 
                    &&
                    m.Transaction_item_id == item.prsp_id &&
                    m.Transaction_item_type == SellingCategory.Produced_Rice_Sales||
                    m.Transaction_item_type==SellingCategory.Produced_Rice_Sales_Remaining
                    ).Sum(m => m.Credit);

                    int action = 0;
                    foreach (var oc in db.Opening_ClosingDays)
                    {
                        if (oc.Date.ToShortDateString() == item.prsp_date.ToShortDateString() && !oc.isClosed)
                        {
                            action = 1;
                            break;
                        }
                    }
                    int invoice_no = db.SaleInvoice.Where(m => m.Sale_id == item.prsp_id && m.SaleInvoice_type == SaleInvoiceType.FactoryRiceSales).SingleOrDefault().SaleInvoice_no;

                    obj.Add(new Tuple<ProducedRiceSales_pt, string, decimal, decimal, decimal, int,int>(item, item.prsp_date.ToShortDateString(), recieved, item.prsp_Total_Amount - recieved, item.prsp_Total_Amount - recieved, action,invoice_no));

                }
                catch (Exception)
                {

                }

            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ReturnStatus(int prsp_id)
        {
            int action = 0;
            try
            {
                var trans = db.Transaction.Where(
                         m => m.Transaction_item_id == prsp_id &&
                         m.Transaction_item_type == SellingCategory.Produced_Rice_Sales_Return
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
        public JsonResult GetPartyRemainings(int Party_Id)
        {
            var produsedSales = db.ProducedRiceSales_pt.Where(m => m.Party_Id == Party_Id && m.prsp_status).ToList();
            var transactions = new List<Transaction>();
            List<Tuple<ProducedRiceSales_pt, decimal, decimal>> jsonret = new List<Tuple<ProducedRiceSales_pt, decimal, decimal>>();
            foreach (var item in produsedSales)
            {
                try
                {
                    decimal recieved = db.Transaction.Where(m => m.status
                   &&
                   m.Transaction_item_id == item.prsp_id &&
                   m.Transaction_item_type == SellingCategory.Produced_Rice_Sales ||
                   m.Transaction_item_type == SellingCategory.Produced_Rice_Sales_Remaining
                   ).Sum(m => m.Credit); 

                    var trans = db.Transaction.Where(
                    m => m.Transaction_item_id == item.prsp_id &&
                    m.Transaction_item_type == SellingCategory.Produced_Rice_Sales_Return
                    ).SingleOrDefault();
                    recieved = (trans != null) ? item.prsp_Total_Amount : recieved;

                    decimal remaining = item.prsp_Total_Amount - recieved;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<ProducedRiceSales_pt, decimal, decimal>(item, recieved, remaining));
                }
                catch (Exception)
                { }
            }
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPartyRemainings1(int Party_Id, int prsp_id)
        {            

            var produsedSales = db.ProducedRiceSales_pt.Where(m => m.Party_Id == Party_Id && m.prsp_status && m.prsp_id != prsp_id).ToList();
            var transactions = new List<Transaction>();
            List<Tuple<ProducedRiceSales_pt, decimal, decimal>> jsonret = new List<Tuple<ProducedRiceSales_pt, decimal, decimal>>();
            foreach (var item in produsedSales)
            {
                try
                {
                    decimal recieved = db.Transaction.Where(m => m.status
                   &&
                   m.Transaction_item_id == item.prsp_id &&
                   m.Transaction_item_type == SellingCategory.Produced_Rice_Sales ||
                   m.Transaction_item_type == SellingCategory.Produced_Rice_Sales_Remaining
                   ).Sum(m => m.Credit);

                    var trans = db.Transaction.Where(
                   m => m.Transaction_item_id == item.prsp_id &&
                   m.Transaction_item_type == SellingCategory.Produced_Rice_Sales_Return
                   ).SingleOrDefault();
                    recieved = (trans != null) ? item.prsp_Total_Amount : recieved;

                    decimal remaining = item.prsp_Total_Amount - recieved;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<ProducedRiceSales_pt, decimal, decimal>(item, recieved, remaining));
                }
                catch (Exception)
                { }
            }
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ch(int id)
        {
            var obj = db.ProducedRiceSales_ch.Where(m => m.prsc_status && m.prsp_id == id).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_Party_Via_Code(FormCollection form)
        {
            try
            {
                var js = new JavaScriptSerializer();
                string Code = JsonConvert.DeserializeObject<string>(form["Code"]);
                if (Code != null)
                {
                    var obj = db.Parties.Where(m => (m.Status) && (m.Party_Code == Code)).SingleOrDefault();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Get_Party_Via_Mobile(FormCollection form)
        {
            try
            {
                var js = new JavaScriptSerializer();
                string Mobile = JsonConvert.DeserializeObject<string>(form["Mobile"]);
                if (Mobile != null)
                {
                    var obj = db.Parties.Where(m => (m.Status) && (m.Party_MobileNo == Mobile)).SingleOrDefault();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Get_party(int prsp_id)
        {
            var obj = db.ProducedRiceSales_pt.Find(prsp_id).Party;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }      


        [HttpPost]
        public JsonResult Get_Produced_Rice()
        {
            var production_rice = db.Rice_Productions.Where(m=>m.Status).ToList();          
            return Json(production_rice, JsonRequestBehavior.AllowGet);
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
            catch(Exception ex)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public JsonResult Get_Price(int id)
        {
            var price = db.Pricing.Where(m => m.item_id == id && m.item_Type == "RawRice" && m.Status).Take(1).SingleOrDefault();
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Return(int prsp_id)
        {
            ProducedRiceSales_pt mProducedRiceSales_pt = db.ProducedRiceSales_pt.Where(m => m.prsp_id == prsp_id && m.prsp_status).SingleOrDefault();
            if (mProducedRiceSales_pt != null)
            {
                foreach (var item in db.ProducedRiceSales_ch.Where(m => m.prsp_id == mProducedRiceSales_pt.prsp_id&& m.prsc_status).ToList())
                {
                    //Again add in stock
                    var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).OrderByDescending(m => m.Rice_Produce_Bag_Date).ToList();

                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        if (item.prsc_qty >= obj1.Rice_Produce_BagsSold && item.prsc_qty > 0)
                        {

                            decimal diff1 = item.prsc_qty - obj1.Rice_Produce_BagsSold;
                            obj1.Rice_Produce_BagsSold -= Convert.ToInt32(obj1.Rice_Produce_BagsSold);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (item.prsc_qty < obj1.Rice_Produce_BagsSold && item.prsc_qty > 0)
                        {
                            decimal diff1 = item.prsc_qty - obj1.Rice_Produce_BagsSold;
                            obj1.Rice_Produce_BagsSold -= Convert.ToInt32(item.prsc_qty);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                    }

                    
                }

                var Receivedtrans = db.Transaction.Where(
                      m => m.status &&
                      m.Transaction_item_id == mProducedRiceSales_pt.prsp_id &&
                          (m.Transaction_item_type == SellingCategory.RawRice_Sales || m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining)
                      ).SingleOrDefault();
                decimal recieved = (Receivedtrans != null) ? Receivedtrans.Credit : 0;

                Transaction trans = new Transaction();
                trans.isByCash = true;
                trans.BankAccountNo = "";
                foreach (var item in db.Opening_ClosingDays)
                {
                    if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                    {
                        trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                        break;
                    }
                }
                trans.Transaction_DateTime = DateTime.Now;
                trans.Transaction_Description = "RawRice Return from " + db.Parties.Find(mProducedRiceSales_pt.Party_Id).Party_Name;
                trans.Transaction_item_id = prsp_id;
                trans.Transaction_item_type = SellingCategory.Produced_Rice_Sales_Return;
                trans.Debit = recieved;
                trans.Credit = 0;
                trans.status = (recieved > 0) ? true : false;
                db.Transaction.Add(trans);
                db.SaveChanges();
            }
            return Json(1, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public void Insert_sales(FormCollection form)
        {
            var js = new JavaScriptSerializer();
            decimal Recieved_Amount = Convert.ToDecimal(form["RecievedAmount"]);
            decimal NetTotal = Convert.ToDecimal(form["NetTotal"]);
            try
            {
                List<int> remaining_rsp_id = js.Deserialize<List<int>>(form["remaining_prsp_id"]);
                for (int i = 0; i < remaining_rsp_id.Count; i++)
                {
                    var RawRice_Sales_pt = db.ProducedRiceSales_pt.Find(remaining_rsp_id[i]);
                    decimal rec = db.Transaction.Where(m => m.status
                     &&
                     m.Transaction_item_id == RawRice_Sales_pt.prsp_id &&
                     m.Transaction_item_type == SellingCategory.Produced_Rice_Sales ||
                     m.Transaction_item_type == SellingCategory.Produced_Rice_Sales_Remaining
                     ).Sum(m => m.Credit);
                    var Remaining = RawRice_Sales_pt.prsp_Total_Amount - rec;

                    Transaction trans = new Transaction();
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
                    foreach (var item in db.Opening_ClosingDays)
                    {
                        if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                        {
                            trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                            break;
                        }
                    }
                    trans.Transaction_DateTime = DateTime.Now;
                    trans.Transaction_Description = "Received Remaining from " + db.Parties.Find(RawRice_Sales_pt.Party_Id).Party_Name;
                    trans.Transaction_item_id = remaining_rsp_id[i];
                    trans.Transaction_item_type = SellingCategory.Produced_Rice_Sales_Remaining;
                    trans.Debit = 0;
                    trans.Credit = Remaining;
                    trans.status = true;
                    db.Transaction.Add(trans);
                    db.SaveChanges();
                    Recieved_Amount -= Remaining;
                }

            }
            catch (Exception ex)
            {

            }

            ProducedRiceSales_pt ProducedRiceSales_pt = js.Deserialize<ProducedRiceSales_pt>(form["ProducedRiceSales_pt"]);
            ProducedRiceSales_pt.prsp_Total_Amount = NetTotal;
            if (ProducedRiceSales_pt.Party.Party_Id > 0)
            {
                ProducedRiceSales_pt.Party_Id = ProducedRiceSales_pt.Party.Party_Id;
                ProducedRiceSales_pt.Party = null;
            }
            else
            {
                //string idd = Convert.ToString(Session["UserId"]);

                string idd = User.Identity.GetUserId();
                ProducedRiceSales_pt.Party.Id = idd;
                ProducedRiceSales_pt.Party.Status = true;
                db.Parties.Add(ProducedRiceSales_pt.Party);
                db.SaveChanges();

                ProducedRiceSales_pt.Party_Id = db.Parties.Max(m => m.Party_Id);
                ProducedRiceSales_pt.Party = null;
            }
            ProducedRiceSales_pt.prsp_Title = ProducedRiceSales_pt.prsp_Title + " to Party " + db.Parties.Find(ProducedRiceSales_pt.Party_Id).Party_Name;
            ProducedRiceSales_pt.prsp_status = true;
            ProducedRiceSales_pt.prsp_date = DateTime.Now;
            db.ProducedRiceSales_pt.Add(ProducedRiceSales_pt);
            db.SaveChanges();
            int prsp_id = db.ProducedRiceSales_pt.Max(m => m.prsp_id);
            List<ProducedRiceSales_ch> ProducedRiceSales_ch = js.Deserialize<ProducedRiceSales_ch[]>(form["ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in ProducedRiceSales_ch)
            {
                item.prsp_id = prsp_id;
                item.prsc_status = true;
                db.ProducedRiceSales_ch.Add(item);
                db.SaveChanges();

                var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m=>m.Rice_Production_id==item.Rice_Production_id).ToList();

                decimal qty = item.prsc_qty;

                foreach (var obj in Rice_Produce_Bags)
                {
                    decimal diff = obj.Rice_Produce_TotalBagsProduce - obj.Rice_Produce_BagsSold;
                    if (diff>qty && qty > 0)
                    {
                        obj.Rice_Produce_BagsSold += Convert.ToInt32(qty);
                        obj.Rice_Produce_RemainingBags = obj.Rice_Produce_TotalBagsProduce - obj.Rice_Produce_BagsSold;
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        qty = 0;
                    }
                    else if (diff <= qty && qty > 0 && diff !=0)
                    {
                        obj.Rice_Produce_BagsSold += Convert.ToInt32(diff);
                        obj.Rice_Produce_RemainingBags = obj.Rice_Produce_TotalBagsProduce - obj.Rice_Produce_BagsSold;
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        qty -= diff;
                    }

                }
                    
                
               
            }
            Transaction trans_this = new Transaction();
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
            foreach (var item in db.Opening_ClosingDays)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    trans_this.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                    break;
                }
            }
            trans_this.Transaction_DateTime = DateTime.Now;
            trans_this.Transaction_Description = ProducedRiceSales_pt.prsp_Title;
            trans_this.Transaction_item_id = prsp_id;
            trans_this.Transaction_item_type = SellingCategory.Produced_Rice_Sales;
            trans_this.Debit = 0;
            trans_this.Credit = Recieved_Amount;
            trans_this.status = true;
            db.Transaction.Add(trans_this);
            db.SaveChanges();

            SaleInvoiceType mSaleInvoiceType = new SaleInvoiceType();
            SaleInvoice saleInvoice = new SaleInvoice()
            {
                SaleInvoice_no = mSaleInvoiceType.GenerateInvoiceNo(SaleInvoiceType.FactoryRiceSales),
                SaleInvoice_type = SaleInvoiceType.FactoryRiceSales,
                Sale_id = db.ProducedRiceSales_pt.Max(m => m.prsp_id)
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
                List<int> remaining_prsp_id = js.Deserialize<List<int>>(form["remaining_prsp_id"]);
                for (int i = 0; i < remaining_prsp_id.Count; i++)
                {
                    var ProducedRiceSales_pt1 = db.ProducedRiceSales_pt.Find(remaining_prsp_id[i]);
                    decimal rec = db.Transaction.Where(m => m.status
                    &&
                    m.Transaction_item_id == ProducedRiceSales_pt1.prsp_id &&
                    m.Transaction_item_type == SellingCategory.Produced_Rice_Sales ||
                    m.Transaction_item_type == SellingCategory.Produced_Rice_Sales_Remaining
                    ).Sum(m => m.Credit); var Remaining = ProducedRiceSales_pt1.prsp_Total_Amount - rec;

                    Transaction trans = new Transaction();
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
                    foreach (var item in db.Opening_ClosingDays)
                    {
                        if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                        {
                            trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                            break;
                        }
                    }
                    trans.Transaction_DateTime = DateTime.Now;
                    trans.Transaction_Description = "Received Remaining from " + db.Parties.Find(ProducedRiceSales_pt1.Party_Id).Party_Name;
                    trans.Transaction_item_id = remaining_prsp_id[i];
                    trans.Transaction_item_type = SellingCategory.Produced_Rice_Sales_Remaining;
                    trans.Debit = 0;
                    trans.Credit = Remaining;
                    trans.status = true;
                    db.Transaction.Add(trans);
                    db.SaveChanges();
                    Recieved_Amount -= Remaining;
                }

            }
            catch (Exception ex)
            {

            }

            ProducedRiceSales_pt ProducedRiceSales_pt_view = js.Deserialize<ProducedRiceSales_pt>(form["ProducedRiceSales_pt"]);
            var ProducedRiceSales_pt = db.ProducedRiceSales_pt.Find(ProducedRiceSales_pt_view.prsp_id);
            if (ProducedRiceSales_pt_view.Party.Party_Id > 0)
            {
                ProducedRiceSales_pt.Party_Id = ProducedRiceSales_pt_view.Party.Party_Id;
                ProducedRiceSales_pt.Party = db.Parties.Find(ProducedRiceSales_pt.Party_Id);
            }
            else
            {
                var mParty = ProducedRiceSales_pt_view.Party;
                //string idd = Convert.ToString(Session["UserId"]);
                string idd = User.Identity.GetUserId();
                mParty.Id = idd;
                mParty.Status = true;
                db.Parties.Add(mParty);
                db.SaveChanges();

                ProducedRiceSales_pt.Party_Id = db.Parties.Max(m => m.Party_Id);
                ProducedRiceSales_pt.Party = db.Parties.Find(ProducedRiceSales_pt.Party_Id);
            }
           // ProducedRiceSales_pt.Party_Id = ProducedRiceSales_pt_view.Party_Id;
            ProducedRiceSales_pt.prsp_TotalWeight_KG = ProducedRiceSales_pt_view.prsp_TotalWeight_KG;
            ProducedRiceSales_pt.prsp_TotalWeight_Mann = ProducedRiceSales_pt_view.prsp_TotalWeight_Mann;
            ProducedRiceSales_pt.prsp_Total_Amount = ProducedRiceSales_pt_view.prsp_Total_Amount;
            ProducedRiceSales_pt.prsp_Title = ProducedRiceSales_pt_view.prsp_Title + " to Party " + db.Parties.Find(ProducedRiceSales_pt_view.Party_Id).Party_Name;
            ProducedRiceSales_pt.prsp_status = true;
            ProducedRiceSales_pt.prsp_date = DateTime.Now;
            db.Entry(ProducedRiceSales_pt).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            int prsp_id = ProducedRiceSales_pt.prsp_id;
            var obj = js.DeserializeObject(form["ProducedRiceSales_ch"].ToString());
            List<ProducedRiceSales_ch> ProducedRiceSales_ch = js.Deserialize<ProducedRiceSales_ch[]>(form["ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in ProducedRiceSales_ch)
            {
                var rsc = db.ProducedRiceSales_ch.Find(item.prsc_id);
                if (rsc == null)
                {
                    rsc = new ProducedRiceSales_ch();
                }
                rsc.Rice_Production_id = item.Rice_Production_id;
                rsc.prsc_price = item.prsc_price;
                rsc.prsc_qty = item.prsc_qty;
                rsc.prsc_status = item.prsc_status;
                rsc.prsc_title = item.prsc_title;
                rsc.prsc_Weight_kg = item.prsc_Weight_kg;
                rsc.prsc_Weight_mann = item.prsc_Weight_mann;
                rsc.prsp_id = prsp_id;

                rsc.prsc_status = true;
                if (item.prsc_id == 0)
                {
                    db.ProducedRiceSales_ch.Add(rsc);
                    db.SaveChanges();


                    var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).ToList();

                    decimal qty = item.prsc_qty;

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
                        prsc_qty = newdb.ProducedRiceSales_ch.Where(m => m.prsc_id == item.prsc_id).First().prsc_qty;
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

                    decimal qty = item.prsc_qty;
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

            List<ProducedRiceSales_ch> Deleted_RawRice_Sales_ch = js.Deserialize<ProducedRiceSales_ch[]>(form["Deleted_ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in Deleted_RawRice_Sales_ch)
            {

                var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).OrderByDescending(m => m.Rice_Produce_Bag_Date).ToList();

                foreach (var obj1 in Rice_Produce_Bags)
                {
                    if (item.prsc_qty >= obj1.Rice_Produce_BagsSold && item.prsc_qty > 0)
                    {

                        decimal diff1 = item.prsc_qty - obj1.Rice_Produce_BagsSold;
                        obj1.Rice_Produce_BagsSold -= Convert.ToInt32(obj1.Rice_Produce_BagsSold);
                        obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                        db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        item.prsc_qty = (int)diff1;
                    }
                    else if (item.prsc_qty < obj1.Rice_Produce_BagsSold && item.prsc_qty > 0)
                    {
                        decimal diff1 = item.prsc_qty - obj1.Rice_Produce_BagsSold;
                        obj1.Rice_Produce_BagsSold -= Convert.ToInt32(item.prsc_qty);
                        obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                        db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        item.prsc_qty = (int)diff1;
                    }

                }
                var prsc = db.ProducedRiceSales_ch.Find(item.prsc_id);
                prsc.prsc_status = false;
                db.Entry(prsc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            foreach (var item in db.Transaction.Where(m => m.status
                    &&
                    m.Transaction_item_id == ProducedRiceSales_pt.prsp_id &&
                    m.Transaction_item_type == SellingCategory.Produced_Rice_Sales 
                    ).ToList())
            {
                db.Transaction.Remove(item);
            }
            db.SaveChanges();
            Transaction trans_this = new Transaction();
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
            foreach (var item in db.Opening_ClosingDays)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    trans_this.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                    break;
                }
            }
            trans_this.Transaction_DateTime = DateTime.Now;
            trans_this.Transaction_Description = ProducedRiceSales_pt.prsp_Title;
            trans_this.Transaction_item_id = prsp_id;
            trans_this.Transaction_item_type = SellingCategory.Produced_Rice_Sales;
            trans_this.Debit = 0;
            trans_this.Credit = Recieved_Amount;
            trans_this.status = true;
            db.Transaction.Add(trans_this);
            db.SaveChanges();
        }
           
        
        
        [HttpPost]
        public void Delete_sales(int id)
        {
            var ProducedRiceSales_pt = db.ProducedRiceSales_pt.Find(id);
            if (ProducedRiceSales_pt != null)
            {
                var ProducedRiceSales_ch = db.ProducedRiceSales_ch.Where(m => m.prsp_id == ProducedRiceSales_pt.prsp_id).ToList();
                foreach (var item in ProducedRiceSales_ch)
                {
                    var Rice_Produce_Bags = db.Rice_Produce_Bags.Where(m => m.Rice_Production_id == item.Rice_Production_id).OrderByDescending(m => m.Rice_Produce_Bag_Date).ToList();

                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        if (item.prsc_qty >= obj1.Rice_Produce_BagsSold && item.prsc_qty > 0)
                        {

                            decimal diff1 = item.prsc_qty - obj1.Rice_Produce_BagsSold;
                            obj1.Rice_Produce_BagsSold -= Convert.ToInt32(obj1.Rice_Produce_BagsSold);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            item.prsc_qty = (int)diff1;
                        }
                        else if (item.prsc_qty < obj1.Rice_Produce_BagsSold && item.prsc_qty > 0)
                        {
                            decimal diff1 = item.prsc_qty - obj1.Rice_Produce_BagsSold;
                            obj1.Rice_Produce_BagsSold -= Convert.ToInt32(item.prsc_qty);
                            obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            item.prsc_qty = (int)diff1;
                        }

                    }

                    item.prsc_status = false;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                ProducedRiceSales_pt.prsp_status = false;
                db.Entry(ProducedRiceSales_pt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                foreach (var item in db.Transaction.Where(m => m.status
                    &&
                    m.Transaction_item_id == ProducedRiceSales_pt.prsp_id &&
                    m.Transaction_item_type == SellingCategory.Produced_Rice_Sales 
                    ).ToList())
                {
                    db.Transaction.Remove(item);
                }
                db.SaveChanges();
            }

        }
        [HttpPost]
        public void ReceivedRemaining(FormCollection form)
        {
            var ProducedRiceSales_pt = db.ProducedRiceSales_pt.Find(Convert.ToInt32(form["id"]));
           // var rec = db.Transaction.Where(m => m.Transaction_item_id == ProducedRiceSales_pt.prsp_id && m.Transaction_item_type == "Produced Rice Sales").Sum(m => m.Credit);
           // var Remaining = ProducedRiceSales_pt.prsp_Total_Amount - rec;

            Transaction trans = new Transaction();

            trans.isByCash = true;
            trans.BankAccountNo = "";

            foreach (var item in db.Opening_ClosingDays)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                    break;
                }
            }
            trans.Transaction_DateTime = DateTime.Now;
            trans.Transaction_Description = "Received Remaining from " + db.Parties.Find(ProducedRiceSales_pt.Party_Id).Party_Name;
            trans.Transaction_item_id = Convert.ToInt32(form["id"]);
            trans.Transaction_item_type = SellingCategory.Produced_Rice_Sales_Remaining;
            trans.Debit = 0;
            trans.Credit = Convert.ToInt32(form["Remaining"]);
            trans.status = true;
            db.Transaction.Add(trans);
            db.SaveChanges();
        }
    }
}