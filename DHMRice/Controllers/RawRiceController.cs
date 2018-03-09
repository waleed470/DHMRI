using CrystalDecisions.CrystalReports.Engine;
using DHMRice.Models;
using DHMRice.Models.HardCode;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DHMRice.Controllers
{
    public class RawRiceController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RawRice
        public ActionResult Index()
        {

            return View(db.RarRices.Where(m => m.Status).ToList());
        }

        public ActionResult Reports()
        {

            return View();
        }
        public ActionResult AddNew()
        {

            ViewBag.Party_Id = new SelectList(db.Parties.Where(m => m.Status == true).ToList(), "Party_Id", "Party_Name");

            ViewBag.Broker_Id = new SelectList(db.Brokers.Where(m => m.Status == true).ToList(), "Broker_Id", "Broker_Name");

            ViewBag.Rice_category_Id = new SelectList(db.Rice_Categories.Where(m => m.Status == true).ToList(), "Rice_category_Id", "Rice_Category_Name");

            ViewBag.Packing_Id = new SelectList(db.Packings.ToList(), "Packing_Id", "Packing_Type");
            return View();
        }
        [HttpPost]
        public ActionResult AddNew(RawRice rawRice, List<RawRiceExpense> RawRiceExpense, Pricing pricing, List<RawRice> RawRice_Remaining, List<decimal> Previous_Remainings, List<int> RawRice_Remaining_checkbox, FormCollection form)
        {
            var Pay_CommissionPercentage = form["Pay_CommissionPercentage"];
            decimal payedamount = Convert.ToDecimal(form["Payed_Amount"]);
            if (RawRice_Remaining != null && Previous_Remainings != null && RawRice_Remaining_checkbox != null)
            {
                for (int i = 0; i < RawRice_Remaining.Count; i++)
                {
                    try
                    {
                        if (RawRice_Remaining_checkbox.Where(m => m.Equals(RawRice_Remaining[i].RawRice_id)).Count() > 0)
                        {
                            Transaction rem_trans = new Transaction();
                            rem_trans.Transaction_item_id = RawRice_Remaining[i].RawRice_id;
                            rem_trans.Transaction_item_type = "RawRice Remaining";
                            rem_trans.Transaction_Description = "Pay Remaining Amount of " + RawRice_Remaining[i].Item_Name + " from Party " + db.Parties.Find(rawRice.Party_Id).Party_Name;
                            rem_trans.Transaction_DateTime = DateTime.Now;
                            if (form["isBankAccount"] != null)
                            {
                                rem_trans.BankAccountNo = form["BankAccountNo"];
                            }
                            else if (form["isCheckbook"] != null)
                            {
                                rem_trans.checkno = Convert.ToInt32(form["CheckNo"]);
                                rem_trans.BankAccountNo = form["BankAccountNo"];
                            }
                            else if (form["isCash"] != null)
                            {
                                rem_trans.isByCash = true;
                                rem_trans.BankAccountNo = "";
                            }
                            foreach (var item in db.Opening_ClosingDays)
                            {
                                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                                {
                                    rem_trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                                    break;
                                }
                            }
                            rem_trans.Debit = Previous_Remainings[i];
                            rem_trans.Credit = 0;
                            rem_trans.status = true;
                            db.Transaction.Add(rem_trans);
                            db.SaveChanges();
                            payedamount = payedamount - Previous_Remainings[i];
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            rawRice.Status = true;
            rawRice.Date = DateTime.Now;

            string idd = Convert.ToString(Session["UserId"]);
            rawRice.Id = idd;
            rawRice.Pb_Weight = rawRice.Total_Weight / rawRice.Bags_qty;
            rawRice.Pay_CommissionPercentage = (Pay_CommissionPercentage == "on") ? true:false;
            rawRice.Bags_Sold_qty = 0;
            db.RarRices.Add(rawRice);
            db.SaveChanges();
            var rawrice_id = db.RarRices.Max(m => m.RawRice_id);

            if (rawRice.Pay_CommissionPercentage)
            {
                Transaction BrokerTransaction = new Transaction();
                if (form["isBankAccount"] != null)
                {
                    BrokerTransaction.BankAccountNo = form["BankAccountNo"];
                }
                else if (form["isCheckbook"] != null)
                {
                    BrokerTransaction.checkno = Convert.ToInt32(form["CheckNo"]);
                    BrokerTransaction.BankAccountNo = form["BankAccountNo"];
                }
                else if (form["isCash"] != null)
                {
                    BrokerTransaction.isByCash = true;
                    BrokerTransaction.BankAccountNo = "";
                }
                foreach (var item in db.Opening_ClosingDays)
                {
                    if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                    {
                        BrokerTransaction.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                        break;
                    }
                }
                BrokerTransaction.Transaction_DateTime = DateTime.Now;
                BrokerTransaction.Transaction_Description = "Pay Commission to Broker "+db.Brokers.Find(db.RarRices.Find(rawrice_id).Broker_Id).Broker_Name+" for RawRice " + rawRice.Item_Name + "  Qty: " + rawRice.Bags_qty + " Packing type: " + db.Packings.Find(rawRice.Packing_Id).Packing_Type + "Kgs from Party " + db.Parties.Find(rawRice.Party_Id).Party_Name;
                BrokerTransaction.Transaction_item_id = rawrice_id;
                BrokerTransaction.Transaction_item_type = BrokerTransactions.PaycommissionToBroker;
                BrokerTransaction.Debit = payedamount;
                BrokerTransaction.Credit = 0;
                BrokerTransaction.status = true;
                db.Transaction.Add(BrokerTransaction);
                db.SaveChanges();
            }

            foreach (var item in RawRiceExpense)
            {
                if (item.RawRiceExpense_Name != null)
                {
                    item.RawRice_id = rawrice_id;
                    db.RawRiceExpense.Add(item);
                    db.SaveChanges();
                }
            }
            pricing.PerBagMarketPrice = pricing.PerBagPrice;
            pricing.item_id = rawrice_id;
            pricing.item_Type = "RawRice";
            pricing.Pricing_Date = DateTime.Now;
            pricing.Pricing_ModifiedDate = DateTime.Now;
            pricing.Status = true;
            db.Pricing.Add(pricing);
            db.SaveChanges();


            Transaction trans = new Transaction();
            if (form["isBankAccount"] != null)
            {
                trans.BankAccountNo = form["BankAccountNo"];
            }
            else if (form["isCheckbook"] != null)
            {
                trans.checkno = Convert.ToInt32(form["CheckNo"]);
                trans.BankAccountNo = form["BankAccountNo"];
            }
            else if (form["isCash"] != null)
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
            trans.Transaction_Description = "Purchase " + rawRice.Item_Name + "  Qty: " + rawRice.Bags_qty + " Packing type: " + db.Packings.Find(rawRice.Packing_Id).Packing_Type + "Kgs from Party " + db.Parties.Find(rawRice.Party_Id).Party_Name;
            trans.Transaction_item_id = rawrice_id;
            trans.Transaction_item_type = "RawRice";
            trans.Debit = payedamount;
            trans.Credit = 0;
            trans.status = true;
            db.Transaction.Add(trans);
            db.SaveChanges();

            return RedirectToAction("GatePassInwawrd", "RawRice", new { id = rawrice_id });
        }
        
        [HttpGet]
        public ActionResult GatePassInwawrd(int id)
        {
           
            var rawrice = db.RarRices.Find(id);

            return View(rawrice);
        }
        [HttpPost]
        public ActionResult GatePassInwawrd(GatePassInwared GatePass, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                
                GatePass.Date = DateTime.Now;
                db.GatePassInwareds.Add(GatePass);


                db.SaveChanges();
                var GatePassInwaredId = db.GatePassInwareds.Max(m => m.GatePassInwaredId);
                return RedirectToAction("GatePass", "RawRice", new { id = GatePassInwaredId });
            }

            return View(GatePass);            
        }

       
        public ActionResult GatePass(int id)
        {

            GatePassInwared Gatepas = db.GatePassInwareds.Where(r=> r.RawRice_id==id).SingleOrDefault();
            db.SaveChanges();
            return View(Gatepas);
        }


        [HttpPost]
        public ActionResult GatePassInwawrdd(int GatePassInwaredId)
        {

            GatePassInwared Gatepas = db.GatePassInwareds.Find(GatePassInwaredId);
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        [HttpPost] 
        public JsonResult GetPartyRemainings(int Party_Id)
        {
            var rawRice = db.RarRices.Where(m => m.Party_Id == Party_Id && m.Status).ToList();
            var transactions = new List<Transaction>();
            List<Tuple<RawRice, decimal>> jsonret = new List<Tuple<RawRice, decimal>>();
            foreach (var item in rawRice)
            {
                try
                {
                    decimal Payed = db.Transaction.Where(m => m.Transaction_item_id == item.RawRice_id &&m.status).Sum(m => m.Debit);
                    decimal remaining = db.Pricing.Where(m => m.item_id == item.RawRice_id && m.Status).Max(m => m.Pricing_NetTotal) - Payed;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<RawRice, decimal>(item, remaining));
                }
                catch(Exception)
                {

                }
               
            }           
           
            //var temp = from rr in db.RarRices
            //           //from tr in db.Transaction
            //           //from pr in db.Pricing
            //           join tr_op in db.Transaction on rr.RawRice_id equals tr_op.Transaction_item_id
            //           join pr_po in db.Pricing on rr.RawRice_id equals pr_po.item_id
            //           where rr.Party_Id == Party_Id && tr_op.Transaction_item_type == "RawRice" && pr_po.item_Type == "RawRice"
            //           select new { rr, tr_op, pr_po };
           
           

            //var RawRice = db.RarRices.Where(m => m.Party_Id == Party_Id /*&& m.pricing.Pricing_NetTotal != m.transaction. */&& m.Status).ToList();
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);           
        }
        [HttpPut]
        public void PayBrokerCommision(int? RawRiceID)
        {
            if (RawRiceID != null)
            {
                RawRice rawRice = db.RarRices.Find(RawRiceID);
                if (rawRice != null && !rawRice.Pay_CommissionPercentage && rawRice.BrokerCommissionPercentage > 0)
                {
                    Transaction BrokerTransaction = new Transaction();
                  
                        BrokerTransaction.isByCash = true;
                        BrokerTransaction.BankAccountNo = "";
                    
                    foreach (var item in db.Opening_ClosingDays)
                    {
                        if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                        {
                            BrokerTransaction.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                            break;
                        }
                    }
                    BrokerTransaction.Transaction_DateTime = DateTime.Now;
                    BrokerTransaction.Transaction_Description = "Pay Commission to Broker " + db.Brokers.Find(db.RarRices.Find(rawRice.RawRice_id).Broker_Id).Broker_Name + " for RawRice " + rawRice.Item_Name + "  Qty: " + rawRice.Bags_qty + " Packing type: " + db.Packings.Find(rawRice.Packing_Id).Packing_Type + "Kgs from Party " + db.Parties.Find(rawRice.Party_Id).Party_Name;
                    BrokerTransaction.Transaction_item_id = rawRice.RawRice_id;
                    BrokerTransaction.Transaction_item_type = BrokerTransactions.PaycommissionToBroker;
                    BrokerTransaction.Debit = rawRice.BrokerCommissionAmount;
                    BrokerTransaction.Credit = 0;
                    BrokerTransaction.status = true;
                    db.Transaction.Add(BrokerTransaction);
                    db.SaveChanges();

                    rawRice.Pay_CommissionPercentage = true;
                    db.Entry(rawRice).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public JsonResult GetPartyRemainings2(int Party_Id,int rawrice_id)
        {
            var rawRice = db.RarRices.Where(m => m.Party_Id == Party_Id&&m.RawRice_id!=rawrice_id && m.Status).ToList();
            var transactions = new List<Transaction>();
            List<Tuple<RawRice, decimal>> jsonret = new List<Tuple<RawRice, decimal>>();
            foreach (var item in rawRice)
            {
                try
                {
                    decimal Payed = db.Transaction.Where(m => m.Transaction_item_id == item.RawRice_id && m.status).Sum(m => m.Debit);
                    decimal remaining = db.Pricing.Where(m => m.item_id == item.RawRice_id && m.Status).Max(m => m.Pricing_NetTotal) - Payed;
                    if (remaining > 0)
                        jsonret.Add(new Tuple<RawRice, decimal>(item, remaining));
                }
                catch (Exception)
                {

                }

            }

            //var temp = from rr in db.RarRices
            //           //from tr in db.Transaction
            //           //from pr in db.Pricing
            //           join tr_op in db.Transaction on rr.RawRice_id equals tr_op.Transaction_item_id
            //           join pr_po in db.Pricing on rr.RawRice_id equals pr_po.item_id
            //           where rr.Party_Id == Party_Id && tr_op.Transaction_item_type == "RawRice" && pr_po.item_Type == "RawRice"
            //           select new { rr, tr_op, pr_po };



            //var RawRice = db.RarRices.Where(m => m.Party_Id == Party_Id /*&& m.pricing.Pricing_NetTotal != m.transaction. */&& m.Status).ToList();
            return Json(jsonret.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void UpdatePrice(Pricing price)
        {
            if (price != null)
            {
                var price_obj = db.Pricing.Find(price.Pricing_id);
                if (price_obj != null)
                {
                    price_obj.PerBagMarketPrice = price.PerBagMarketPrice;
                    db.Entry(price_obj).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public ActionResult Edit(int? id)
        {        
            var RawRice = db.RarRices.Find(id);
            if (RawRice != null)
            {
                ViewBag.Party_Id = new SelectList(db.Parties.Where(m => m.Status == true).ToList(), "Party_Id", "Party_Name",RawRice.Party_Id);

                ViewBag.Broker_Id = new SelectList(db.Brokers.Where(m => m.Status == true).ToList(), "Broker_Id", "Broker_Name",RawRice.Broker_Id);

                ViewBag.Rice_category_Id = new SelectList(db.Rice_Categories.Where(m => m.Status == true).ToList(), "Rice_category_Id", "Rice_Category_Name",RawRice.Rice_category_Id);

                ViewBag.Packing_Id = new SelectList(db.Packings.ToList(), "Packing_Id", "Packing_Type");
                return View(RawRice);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit(RawRice rawRice, List<RawRiceExpense> RawRiceExpense, Pricing pricing, List<RawRice> RawRice_Remaining, List<decimal> Previous_Remainings, List<int> RawRice_Remaining_checkbox,List<int> Expense_id_delete, FormCollection form)
        {
            var Pay_CommissionPercentage = form["Pay_CommissionPercentage"];
            decimal payedamount = Convert.ToDecimal(form["Payed_Amount"]);
            if (RawRice_Remaining != null && Previous_Remainings != null && RawRice_Remaining_checkbox != null)
            {
                for (int i = 0; i < RawRice_Remaining.Count; i++)
                {
                    try
                    {
                        if (RawRice_Remaining_checkbox.Where(m => m.Equals(RawRice_Remaining[i].RawRice_id)).Count() > 0)
                        {
                            Transaction rem_trans = new Transaction();
                            rem_trans.Transaction_item_id = RawRice_Remaining[i].RawRice_id;
                            rem_trans.Transaction_item_type = "RawRice Remaining";
                            rem_trans.Transaction_Description = "Pay Remaining Amount of " + RawRice_Remaining[i].Item_Name + " from Party " + db.Parties.Find(rawRice.Party_Id).Party_Name;
                            rem_trans.Transaction_DateTime = DateTime.Now;
                            if (form["isBankAccount"] != null)
                            {
                                rem_trans.BankAccountNo = form["BankAccountNo"];
                            }
                            else if (form["isCheckbook"] != null)
                            {
                                rem_trans.checkno = Convert.ToInt32(form["CheckNo"]);
                                rem_trans.BankAccountNo = form["BankAccountNo"];
                            }
                            else if (form["isCash"] != null)
                            {
                                rem_trans.isByCash = true;
                                rem_trans.BankAccountNo = "";
                            }
                            foreach (var item in db.Opening_ClosingDays)
                            {
                                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                                {
                                    rem_trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                                    break;
                                }
                            }
                            rem_trans.Debit = Previous_Remainings[i];
                            rem_trans.Credit = 0;
                            rem_trans.status = true;
                            db.Transaction.Add(rem_trans);
                            payedamount = payedamount - Previous_Remainings[i];
                            db.SaveChanges();

                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            string idd = Convert.ToString(Session["UserId"]);
            rawRice.Id = idd;
            rawRice.Status = true;
            rawRice.Date = DateTime.Now;
            rawRice.Bags_Sold_qty = 0;
            rawRice.Pay_CommissionPercentage = (Pay_CommissionPercentage == "on") ? true : false;
            db.Entry(rawRice).State = EntityState.Modified;
            db.SaveChanges();

            // var rawrice_id = db.RarRices.Max(m => m.RawRice_id);

            if (RawRiceExpense != null)
            {
                foreach (var item in RawRiceExpense)
                {
                    if (item.RawRiceExpense_Name != null)
                    {
                        item.RawRice_id = rawRice.RawRice_id;
                        if (item.RawRiceExpense_id == 0) { db.RawRiceExpense.Add(item); }
                        else { db.Entry(item).State = EntityState.Modified; }

                    }
                }
            }
         
            if (Expense_id_delete != null)
            {
                foreach (var item in Expense_id_delete)
                {
                    RawRiceExpense expense = db.RawRiceExpense.Find(item);
                    db.RawRiceExpense.Remove(expense);
                }
                db.SaveChanges();
            }
            pricing.PerBagMarketPrice = pricing.PerBagPrice;
            pricing.item_id = rawRice.RawRice_id;
            pricing.item_Type = "RawRice";
            pricing.Pricing_Date = DateTime.Now;
            pricing.Pricing_ModifiedDate = DateTime.Now;
            pricing.Status = true;
            db.Entry(pricing).State = EntityState.Modified;
            db.SaveChanges();


            foreach (var item in db.Transaction.Where(m => m.Transaction_item_id == rawRice.RawRice_id && (m.Transaction_item_type == "RawRice"||m.Transaction_item_type==BrokerTransactions.PaycommissionToBroker ) && m.status))
            {
                db.Transaction.Remove(item);
            }
            db.SaveChanges();
            if (rawRice.Pay_CommissionPercentage)
            {
                Transaction BrokerTransaction = new Transaction();
                if (form["isBankAccount"] != null)
                {
                    BrokerTransaction.BankAccountNo = form["BankAccountNo"];
                }
                else if (form["isCheckbook"] != null)
                {
                    BrokerTransaction.checkno = Convert.ToInt32(form["CheckNo"]);
                    BrokerTransaction.BankAccountNo = form["BankAccountNo"];
                }
                else if (form["isCash"] != null)
                {
                    BrokerTransaction.isByCash = true;
                    BrokerTransaction.BankAccountNo = "";
                }
                foreach (var item in db.Opening_ClosingDays)
                {
                    if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                    {
                        BrokerTransaction.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                        break;
                    }
                }
                BrokerTransaction.Transaction_DateTime = DateTime.Now;
                BrokerTransaction.Transaction_Description = "Pay Commission to Broker " + db.Brokers.Find(db.RarRices.Find(rawRice.RawRice_id).Broker_Id).Broker_Name + " for RawRice " + rawRice.Item_Name + "  Qty: " + rawRice.Bags_qty + " Packing type: " + db.Packings.Find(rawRice.Packing_Id).Packing_Type + "Kgs from Party " + db.Parties.Find(rawRice.Party_Id).Party_Name;
                BrokerTransaction.Transaction_item_id = rawRice.RawRice_id;
                BrokerTransaction.Transaction_item_type = BrokerTransactions.PaycommissionToBroker;
                BrokerTransaction.Debit = payedamount;
                BrokerTransaction.Credit = 0;
                BrokerTransaction.status = true;
                db.Transaction.Add(BrokerTransaction);
                db.SaveChanges();
            }
            Transaction trans = new Transaction();
            if (form["isBankAccount"] != null)
            {
                trans.BankAccountNo = db.Parties.Find(rawRice.Party_Id).Party_ACcountNo;
            }
            else if (form["isCheckbook"] != null)
            {
                trans.checkno = Convert.ToInt32(form["CheckNo"]);
                trans.BankAccountNo = db.Parties.Find(rawRice.Party_Id).Party_ACcountNo;
            }
            else if (form["isCash"] != null)
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
            trans.Transaction_Description = "Purchase " + rawRice.Item_Name + "  Qty: " + rawRice.Bags_qty + " Packing type: " + db.Packings.Find(rawRice.Packing_Id).Packing_Type + "Kgs from Party " + db.Parties.Find(rawRice.Party_Id).Party_Name;

            trans.Transaction_item_id = rawRice.RawRice_id;
            trans.Transaction_item_type = "RawRice";
            trans.Debit = payedamount;
            trans.Credit = 0;
            trans.status = true;
            db.Transaction.Add(trans);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            var RawRice = db.RarRices.Find(id);
            if (RawRice != null)
            {
                db.RarRices.Remove(RawRice);
                foreach (var item in db.RawRiceExpense.Where(m=>m.RawRice_id==RawRice.RawRice_id).ToList())
                {
                    db.RawRiceExpense.Remove(item);                    
                }

                var pricing = db.Pricing.Where(m => m.item_id == RawRice.RawRice_id && m.item_Type == "RawRice" && m.Status).Take(1).SingleOrDefault();
                if(pricing!=null)
                {
                    db.Pricing.Remove(pricing);
                }
                var trans = db.Transaction.Where(m => m.Transaction_item_id == RawRice.RawRice_id && (m.Transaction_item_type == "RawRice"||m.Transaction_item_type==BrokerTransactions.PaycommissionToBroker) && m.status).ToList();
                foreach (var item in trans)
                {
                    db.Transaction.Remove(item);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
        public ActionResult AllRice()
        {

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "All_Rice.rpt"));
            var RawRice = db.RarRices.ToList();
            rd.SetDataSource(RawRice);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "All_Rice.pdf");
        }
        [HttpGet]
        public void MonthWisePerbagMarketPriceUpdation()
        {
            var RawRices = db.RarRices.Where(m => m.Status).ToList();
            foreach (var item in RawRices)
            {
                Pricing pricing = db.Pricing.Where(m => m.item_id == item.RawRice_id && m.item_Type == "RawRice").First();
                if (pricing != null)
                {
                    var m = pricing.Pricing_ModifiedDate.AddMonths(1);
                    if (DateTime.Now >= m)
                    {
                        pricing.PerBagMarketPrice += 15;
                        pricing.Pricing_ModifiedDate = DateTime.Now;
                        db.Entry(pricing).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}