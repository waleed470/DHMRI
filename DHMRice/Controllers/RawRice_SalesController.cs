using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHMRice.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using DHMRice.Models.HardCode;

namespace DHMRice.Controllers
{

    [Authorize(Roles = "Factory,Admin")]
    public class RawRice_SalesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: RawRice_Sales
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Get_pt()
        {
            List<Tuple<RawRice_Sales_pt, string, decimal, decimal,decimal, int, int>> obj = new List<Tuple<RawRice_Sales_pt, string, decimal, decimal, decimal, int,int>>();
            var list = db.RawRice_Sales_pt.Where(m => m.rsp_status).ToList();
            foreach (var item in list)
            {
                try
                {
                    decimal recieved = db.Transaction.Where(
                       m => m.status &&                       
                           m.Transaction_item_id == item.rsp_id && 
                           (m.Transaction_item_type == SellingCategory.RawRice_Sales ||m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining)
                       ).Sum(m => m.Credit);
                    int action = 0;

                    foreach (var oc in db.Opening_ClosingDays)
                    {
                        if (oc.Date.ToShortDateString() == item.rsp_date.ToShortDateString() && !oc.isClosed)
                        {
                            action = 1;
                            break;
                        }
                    }          
                    int invoice_no = db.SaleInvoice.Where(m => m.Sale_id == item.rsp_id && m.SaleInvoice_type == SaleInvoiceType.FactoryRiceSales).SingleOrDefault().SaleInvoice_no;
                    obj.Add(new Tuple<RawRice_Sales_pt, string, decimal, decimal, decimal, int, int>(item, item.rsp_date.ToShortDateString(), recieved, item.rsp_Total_Amount - recieved, item.rsp_Total_Amount - recieved,action,invoice_no));

                }
                catch (Exception ex)
                {

                }

            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost] 
        public JsonResult Get_ReturnStatus(int rsp_id)
        {
            int action = 0;
            try
            {
                var trans = db.Transaction.Where(
                         m => m.Transaction_item_id == rsp_id &&
                         m.Transaction_item_type == SellingCategory.RawRice_Sales_Return
                         ).SingleOrDefault();
                action = (trans!=null) ? 1 : 0;
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
            var rawRice = db.RawRice_Sales_pt.Where(m => m.Party_Id == Party_Id && m.rsp_status).ToList();
            var transactions = new List<Transaction>();
            List<Tuple<RawRice_Sales_pt, decimal, decimal>> jsonret = new List<Tuple<RawRice_Sales_pt, decimal, decimal>>();
            foreach (var item in rawRice)
            {
                try
                {
                    decimal recieved = db.Transaction.Where(
                         m => m.status &&
                         m.Transaction_item_id == item.rsp_id &&
                         m.Transaction_item_type == SellingCategory.RawRice_Sales ||
                         m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining
                         ).Sum(m => m.Credit);

                    
                        var trans = db.Transaction.Where(
                        m => m.Transaction_item_id == item.rsp_id &&
                        m.Transaction_item_type == SellingCategory.RawRice_Sales_Return
                        ).SingleOrDefault();
                        recieved = (trans != null) ? item.rsp_Total_Amount : recieved;
                    

                    decimal remaining = item.rsp_Total_Amount - recieved;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<RawRice_Sales_pt, decimal, decimal>(item, recieved, remaining));
                }
                catch (Exception)
                { }
            }
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartyRemainings1(int Party_Id, int rsp_id)
        {
            var rawRice = db.RawRice_Sales_pt.Where(m => m.Party_Id == Party_Id && m.rsp_status && m.rsp_id != rsp_id).ToList();
            var transactions = new List<Transaction>();
            List<Tuple<RawRice_Sales_pt, decimal, decimal>> jsonret = new List<Tuple<RawRice_Sales_pt, decimal, decimal>>();
            foreach (var item in rawRice)
            {
                try
                {
                    decimal recieved = db.Transaction.Where(
                          m => m.status &&
                          m.Transaction_item_id == item.rsp_id &&
                          m.Transaction_item_type == SellingCategory.RawRice_Sales ||
                          m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining
                          ).Sum(m => m.Credit);
                    decimal remaining = item.rsp_Total_Amount - recieved;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<RawRice_Sales_pt, decimal, decimal>(item, recieved, remaining));
                }
                catch (Exception)
                { }
            }
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ch(int id)
        {
            var obj = db.RawRice_Sales_ch.Where(m => m.rsc_status && m.rsp_id == id).ToList();
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
        public JsonResult Get_party(int rsp_id)
        {
            var obj = db.RawRice_Sales_pt.Find(rsp_id).Party;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_RawRice()
        {
            var obj = db.RarRices.Where(m => m.Status).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_RawRice_id(int id)
        {
            var obj = db.RarRices.Where(m => m.Status && m.RawRice_id == id).SingleOrDefault();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_Price(int id)
        {
            var price = db.Pricing.Where(m => m.item_id == id && m.item_Type == "RawRice" && m.Status).Take(1).SingleOrDefault();
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Return(int rsp_id)
        {
            RawRice_Sales_pt mRawRice_Sales_pt = db.RawRice_Sales_pt.Where(m=>m .rsp_id ==rsp_id && m.rsp_status).SingleOrDefault();
            if (mRawRice_Sales_pt != null)
            {
                foreach (var item in db.RawRice_Sales_ch.Where(m => m.rsp_id == mRawRice_Sales_pt.rsp_id && m.rsc_status).ToList())
                {
                    //Again add in stock
                    var RawRice = db.RarRices.Find(item.RawRice_id);
                    RawRice.Bags_Sold_qty -= item.rsc_qty;
                    db.Entry(RawRice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                decimal recieved = db.Transaction.Where(
                      m => m.status &&
                      m.Transaction_item_id == mRawRice_Sales_pt.rsp_id &&
                          (m.Transaction_item_type == SellingCategory.RawRice_Sales || m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining)
                      ).Sum(m => m.Credit);
                
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
                    trans.Transaction_Description = "RawRice Return from " + db.Parties.Find(mRawRice_Sales_pt.Party_Id).Party_Name;
                    trans.Transaction_item_id = rsp_id;
                    trans.Transaction_item_type = SellingCategory.RawRice_Sales_Return;
                    trans.Debit = recieved;
                    trans.Credit = 0;                
                    trans.status =(recieved>0)? true:false;
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
            try
            {
                List<int> remaining_rsp_id = js.Deserialize<List<int>>(form["remaining_rsp_id"]);
                for (int i = 0; i < remaining_rsp_id.Count; i++)
                {
                    var RawRice_Sales_pt = db.RawRice_Sales_pt.Find(remaining_rsp_id[i]);
                    decimal recieved = db.Transaction.Where(
                           m => m.status &&
                           m.Transaction_item_id == RawRice_Sales_pt.rsp_id &&
                           m.Transaction_item_type == SellingCategory.RawRice_Sales ||
                           m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining
                           ).Sum(m => m.Credit);
                    var Remaining = RawRice_Sales_pt.rsp_Total_Amount - recieved;

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
                    trans.Transaction_item_type = SellingCategory.RawRice_Sales_Remaining;
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
           
            RawRice_Sales_pt RawRiceSales = js.Deserialize<RawRice_Sales_pt>(form["rawRice_Sales_pt"]);
            if (RawRiceSales.Party.Party_Id > 0)
            {
                RawRiceSales.Party_Id = RawRiceSales.Party.Party_Id;
                RawRiceSales.Party = null;
            }
            else
            {
               // string idd = Convert.ToString(Session["UserId"]);
                string idd = User.Identity.GetUserId();
                RawRiceSales.Party.Id = idd;
                RawRiceSales.Party.Status = true;
                db.Parties.Add(RawRiceSales.Party);
                db.SaveChanges();

                RawRiceSales.Party_Id = db.Parties.Max(m => m.Party_Id);
                RawRiceSales.Party = null;
            }
            RawRiceSales.rsp_Title = RawRiceSales.rsp_Title + " to Party " + db.Parties.Find(RawRiceSales.Party_Id).Party_Name;
            RawRiceSales.rsp_status = true;
            RawRiceSales.rsp_date = DateTime.Now;
            db.RawRice_Sales_pt.Add(RawRiceSales);
            db.SaveChanges();
            int rsp_id = db.RawRice_Sales_pt.Max(m => m.rsp_id);
            List<RawRice_Sales_ch> RawRice_Sales_ch = js.Deserialize<RawRice_Sales_ch[]>(form["RawRice_Sales_ch"].ToString()).ToList();
            foreach (var item in RawRice_Sales_ch)
            {
                item.rsp_id = rsp_id;
                item.rsc_status = true;
                db.RawRice_Sales_ch.Add(item);
                db.SaveChanges();

                var RawRice = db.RarRices.Find(item.RawRice_id);
                RawRice.Bags_Sold_qty += item.rsc_qty;
                db.Entry(RawRice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
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
            trans_this.Transaction_Description = RawRiceSales.rsp_Title;
            trans_this.Transaction_item_id = rsp_id;
            trans_this.Transaction_item_type = SellingCategory.RawRice_Sales;
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
                Sale_id = db.RawRice_Sales_pt.Max(m => m.rsp_id)
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
                List<int> remaining_rsp_id = js.Deserialize<List<int>>(form["remaining_rsp_id"]);
                for (int i = 0; i < remaining_rsp_id.Count; i++)
                {
                    var RawRice_Sales_pt = db.RawRice_Sales_pt.Find(remaining_rsp_id[i]);
                    decimal recieved = db.Transaction.Where(
                           m => m.status &&
                           m.Transaction_item_id == RawRice_Sales_pt.rsp_id &&
                           m.Transaction_item_type == SellingCategory.RawRice_Sales ||
                           m.Transaction_item_type == SellingCategory.RawRice_Sales_Remaining
                           ).Sum(m => m.Credit);
                    var Remaining = RawRice_Sales_pt.rsp_Total_Amount - recieved;

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
                    trans.Transaction_item_type = SellingCategory.RawRice_Sales_Remaining;
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

            RawRice_Sales_pt RawRiceSales_View = js.Deserialize<RawRice_Sales_pt>(form["rawRice_Sales_pt"]);
            var RawRiceSales = db.RawRice_Sales_pt.Find(RawRiceSales_View.rsp_id);
            if (RawRiceSales_View.Party.Party_Id > 0)
            {
                RawRiceSales.Party_Id = RawRiceSales_View.Party.Party_Id;
                RawRiceSales.Party = db.Parties.Find(RawRiceSales.Party_Id);
            }
            else
            {
                var mParty = RawRiceSales_View.Party;
                //string idd = Convert.ToString(Session["UserId"]);
                string idd = User.Identity.GetUserId();
                mParty.Id = idd;
                mParty.Status = true;
                db.Parties.Add(mParty);
                db.SaveChanges();

                RawRiceSales.Party_Id = db.Parties.Max(m => m.Party_Id);
                RawRiceSales.Party = db.Parties.Find(RawRiceSales.Party_Id);
            }
           // RawRiceSales.Party_Id = RawRiceSales_View.Party_Id;
            RawRiceSales.rsp_TotalWeight_KG = RawRiceSales_View.rsp_TotalWeight_KG;
            RawRiceSales.rsp_TotalWeight_Mann = RawRiceSales_View.rsp_TotalWeight_Mann;
            RawRiceSales.rsp_Total_Amount = RawRiceSales_View.rsp_Total_Amount;
            RawRiceSales.rsp_Title = RawRiceSales_View.rsp_Title + " to Party " + db.Parties.Find(RawRiceSales.Party_Id).Party_Name;
            RawRiceSales.rsp_status = true;
            RawRiceSales.rsp_date = DateTime.Now;
            db.Entry(RawRiceSales).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            int rsp_id = RawRiceSales.rsp_id;
            var obj = js.DeserializeObject(form["RawRice_Sales_ch"].ToString());
            List<RawRice_Sales_ch> RawRice_Sales_ch = js.Deserialize<RawRice_Sales_ch[]>(form["RawRice_Sales_ch"].ToString()).ToList();
            foreach (var item in RawRice_Sales_ch)
            {
                var rsc = db.RawRice_Sales_ch.Find(item.rsc_id);
                if (rsc == null)
                {
                    rsc = new RawRice_Sales_ch();
                }
                rsc.RawRice_id = item.RawRice_id;
                rsc.rsc_id = item.rsc_id;
                rsc.rsc_price = item.rsc_price;
                rsc.rsc_qty = item.rsc_qty;
                rsc.rsc_status = item.rsc_status;
                rsc.rsc_title = item.rsc_title;
                rsc.rsc_Weight_kg = item.rsc_Weight_kg;
                rsc.rsc_Weight_mann = item.rsc_Weight_mann;
                rsc.rsp_id = rsp_id;

                rsc.rsc_status = true;
                if (rsc.rsc_id == 0)
                {
                    db.RawRice_Sales_ch.Add(rsc);
                    db.SaveChanges();

                    var RawRice = db.RarRices.Find(rsc.RawRice_id);
                    RawRice.Bags_Sold_qty += rsc.rsc_qty;
                    db.Entry(RawRice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    int rsc_qty = 0;
                    using (var newdb = new ApplicationDbContext())
                    {
                        rsc_qty = newdb.RawRice_Sales_ch.Where(m => m.rsc_id == item.rsc_id).Take(1).Single().rsc_qty;
                    }
                    var RawRice = db.RarRices.Find(rsc.RawRice_id);
                    RawRice.Bags_Sold_qty -= rsc_qty;
                    db.Entry(RawRice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    db.Entry(rsc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    RawRice = db.RarRices.Find(rsc.RawRice_id);
                    RawRice.Bags_Sold_qty += rsc.rsc_qty;
                    db.Entry(RawRice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            List<RawRice_Sales_ch> Deleted_RawRice_Sales_ch = js.Deserialize<RawRice_Sales_ch[]>(form["Deleted_RawRice_Sales_ch"].ToString()).ToList();
            foreach (var item in Deleted_RawRice_Sales_ch)
            {
                var RawRice = db.RarRices.Find(item.RawRice_id);
                RawRice.Bags_Sold_qty -= item.rsc_qty;
                db.Entry(RawRice).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                var rsc = db.RawRice_Sales_ch.Find(item.rsc_id);
                rsc.rsc_status = false;
                db.Entry(rsc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            foreach (var item in db.Transaction.Where(
                          m => m.status &&
                          m.Transaction_item_id == RawRiceSales.rsp_id &&
                          m.Transaction_item_type == SellingCategory.RawRice_Sales 
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
            trans_this.Transaction_Description = RawRiceSales.rsp_Title;
            trans_this.Transaction_item_id = rsp_id;
            trans_this.Transaction_item_type = SellingCategory.RawRice_Sales;
            trans_this.Debit = 0;
            trans_this.Credit = Recieved_Amount;
            trans_this.status = true;
            db.Transaction.Add(trans_this);
            db.SaveChanges();
        }
        [HttpPost]
        public void Delete_sales(int id)
        {
            var Raw_pt = db.RawRice_Sales_pt.Find(id);
            if (Raw_pt != null)
            {
                var Raw_ch = db.RawRice_Sales_ch.Where(m => m.rsp_id == Raw_pt.rsp_id).ToList();
                foreach (var item in Raw_ch)
                {
                    var RawRice = db.RarRices.Find(item.RawRice_id);
                    RawRice.Bags_Sold_qty -= item.rsc_qty;
                    db.Entry(RawRice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    item.rsc_status = false;
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                Raw_pt.rsp_status = false;
                db.Entry(Raw_pt).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                foreach (var item in db.Transaction.Where(
                          m => m.status &&
                          m.Transaction_item_id == Raw_pt.rsp_id &&
                          m.Transaction_item_type == SellingCategory.RawRice_Sales
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
            var RawRice_Sales_pt = db.RawRice_Sales_pt.Find(Convert.ToInt32(form["id"]));
            //var rec = db.Transaction.Where(m => m.Transaction_item_id == RawRice_Sales_pt.rsp_id && m.Transaction_item_type == "RawRice Sales").Sum(m => m.Credit);
            //var Remaining = RawRice_Sales_pt.rsp_Total_Amount - rec;

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
            trans.Transaction_Description = "Received Remaining from " + db.Parties.Find(RawRice_Sales_pt.Party_Id).Party_Name;
            trans.Transaction_item_id = Convert.ToInt32(form["id"]);
            trans.Transaction_item_type = SellingCategory.RawRice_Sales;
            trans.Debit = 0;
            trans.Credit = Convert.ToInt32(form["Remaining"]);
            trans.status = true;
            db.Transaction.Add(trans);
            db.SaveChanges();
        }
    }
}