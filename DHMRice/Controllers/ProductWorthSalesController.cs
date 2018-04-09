using DHMRice.Models;
using DHMRice.Models.HardCode;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        public ActionResult GatePassOutward(int id)
        {
            var gate = db.GatePassOutward.Where(g => g.RiceTypeId == id && g.RiceType == "Worth Rice Sales").SingleOrDefault();
            if (gate == null)
            {
                var rawsale = db.BpRiceSales_pts.Find(id);
                return View(rawsale);
            }
            else
            {
                return RedirectToAction("GatePass", "ProductWorthSales", new { id = id });
            }
        }
        [HttpPost]
        public ActionResult GatePassOutward(GatePassOutward GatePass, FormCollection form)
        {
            if (ModelState.IsValid)
            {

                GatePass.Date = DateTime.Now;
                GatePass.RiceType = "Worth Rice Sales";
                db.GatePassOutward.Add(GatePass);


                db.SaveChanges();
                var GatePassId = GatePass.RiceTypeId;
                return RedirectToAction("GatePass", "ProductWorthSales", new { id = GatePassId });
            }

            return View(GatePass);
        }
        public ActionResult GatePass(int id)
        {

            GatePassOutward Gatepas = db.GatePassOutward.Where(r => r.RiceTypeId == id && r.RiceType == "Worth Rice Sales").SingleOrDefault();
            db.SaveChanges();
            return View(Gatepas);
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
        public JsonResult Get_pt()
        {
            List<Tuple<BpRiceSales_pt, string, decimal, decimal, decimal, int, int>> obj = new List<Tuple<BpRiceSales_pt, string, decimal, decimal, decimal, int, int>>();
            var list = db.BpRiceSales_pts.Where(m => m.bprsp_status).ToList();
            foreach (var item in list)
            {
                try
                {
                    decimal recieved = db.Transaction.Where(m => m.status
                   &&
                   m.Transaction_item_id == item.bprsp_id &&
                   m.Transaction_item_type == SellingCategory.Worth_Rice_Sales ||
                   m.Transaction_item_type == SellingCategory.Worth_Rice_Sales_Remaining
                   ).Sum(m => m.Credit);

                    int action = 0;
                    foreach (var oc in db.Opening_ClosingDays)
                    {
                        if (oc.Date.ToShortDateString() == item.bprsp_date.ToShortDateString() && !oc.isClosed)
                        {
                            action = 1;
                            break;
                        }
                    }
                    int invoice_no = db.SaleInvoice.Where(m => m.Sale_id == item.bprsp_id && m.SaleInvoice_type == SaleInvoiceType.FactoryRiceSales).SingleOrDefault().SaleInvoice_no;

                    obj.Add(new Tuple<BpRiceSales_pt, string, decimal, decimal, decimal, int, int>(item, item.bprsp_date.ToShortDateString(), recieved, item.bprsp_Total_Amount - recieved, item.bprsp_Total_Amount - recieved, action, invoice_no));

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
                         m.Transaction_item_type == SellingCategory.Worth_Rice_Sales_Return
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
        public JsonResult Get_Party(int prsp_id)
        {
            var obj = db.BpRiceSales_pts.Find(prsp_id).Party;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_ch(int id)
        {
            var obj = db.BpRiceSales_chs.Where(m => m.bprsc_status && m.bprsp_id == id).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get_Produced_Rice_id(int id)
        {
            try
            {
                Rice_Production_ProductWorth worth = db.Rice_Production_ProductWorths.Find(id);

                Rice_Produce_Bag bagid = db.Rice_Produce_Bags.Find(worth.Rice_Produce_Bags_id);
                int Rice_Production_id = bagid.Rice_Production_id;
                decimal qty = worth.Rice_Production_ProductWorth_Qty;
                decimal Soldqty = worth.Rice_Production_ProductWorth_SoldQty;
                decimal PBP = worth.Rice_Production_ProductWorth_PBA;


                int Rice_Production_ProductWorth_id = id;
                string bprsc_title = worth.Rice_Production_ProductWorth_name;
                int worthid = worth.Rice_Production_ProductWorth_id;
             
                Tuple<decimal, int, decimal, string,decimal,int,int> tpl = new Tuple<decimal, int, decimal, string,decimal,int,int>
                (
                qty,
                Rice_Production_ProductWorth_id,
                Soldqty,
                bprsc_title,
                PBP,
                worthid,
                Rice_Production_id
                );

                return Json(tpl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

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
                     m.Transaction_item_type == SellingCategory.Worth_Rice_Sales ||
                     m.Transaction_item_type == SellingCategory.Worth_Rice_Sales_Remaining
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
                    trans.Transaction_item_type = SellingCategory.Worth_Rice_Sales_Remaining;
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

            BpRiceSales_pt ProducedRiceSales_pt = js.Deserialize<BpRiceSales_pt>(form["ProducedRiceSales_pt"]);
            ProducedRiceSales_pt.bprsp_Total_Amount = NetTotal;
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
            ProducedRiceSales_pt.bprsp_Title = ProducedRiceSales_pt.bprsp_Title + " to Party " + db.Parties.Find(ProducedRiceSales_pt.Party_Id).Party_Name;
            ProducedRiceSales_pt.bprsp_status = true;
            ProducedRiceSales_pt.bprsp_date = DateTime.Now;
            db.BpRiceSales_pts.Add(ProducedRiceSales_pt);
            db.SaveChanges();
            int prsp_id = db.BpRiceSales_pts.Max(m => m.bprsp_id);
            List<BpRiceSales_ch> ProducedRiceSales_ch = js.Deserialize<BpRiceSales_ch[]>(form["ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in ProducedRiceSales_ch)
            {
                item.bprsp_id = prsp_id;
                item.bprsc_status = true;
                db.BpRiceSales_chs.Add(item);
                db.SaveChanges();

                var Rice_Produce_Bags = db.Rice_Production_ProductWorths.Where(m => m.Rice_Production_ProductWorth_id == item.Rice_Production_ProductWorth_id).ToList();

                decimal qty = item.bprsc_qty;

                foreach (var obj in Rice_Produce_Bags)
                {
                    decimal diff = obj.Rice_Production_ProductWorth_Qty - obj.Rice_Production_ProductWorth_SoldQty;
                    if (diff > qty && qty > 0)
                    {
                        obj.Rice_Production_ProductWorth_SoldQty += Convert.ToInt32(qty);
                        //obj.Rice_Produce_RemainingBags = obj.Rice_Production_ProductWorth_Qty - obj.Rice_Production_ProductWorth_SoldQty;
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        qty = 0;
                    }
                    else if (diff <= qty && qty > 0 && diff != 0)
                    {
                        obj.Rice_Production_ProductWorth_SoldQty += Convert.ToInt32(diff);
                        //obj.Rice_Produce_RemainingBags = obj.Rice_Produce_TotalBagsProduce - obj.Rice_Produce_BagsSold;
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
            trans_this.Transaction_Description = ProducedRiceSales_pt.bprsp_Title;
            trans_this.Transaction_item_id = prsp_id;
            trans_this.Transaction_item_type = SellingCategory.Worth_Rice_Sales;
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
                Sale_id = db.BpRiceSales_pts.Max(m => m.bprsp_id)
            };
            db.SaleInvoice.Add(saleInvoice);
            db.SaveChanges();
        }

        [HttpPost]
        public JsonResult Return(int prsp_id)
        {
            BpRiceSales_pt mProducedRiceSales_pt = db.BpRiceSales_pts.Where(m => m.bprsp_id == prsp_id && m.bprsp_status).SingleOrDefault();
            if (mProducedRiceSales_pt != null)
            {
                foreach (var item in db.BpRiceSales_chs.Where(m => m.bprsp_id == mProducedRiceSales_pt.bprsp_id && m.bprsc_status).ToList())
                {
                    //Again add in stock
                    var Rice_Produce_Bags = db.Rice_Production_ProductWorths.Where(m => m.Rice_Production_ProductWorth_id == item.Rice_Production_ProductWorth_id).OrderByDescending(m => m.Rice_Produce_Bag.Rice_Produce_Bag_Date).ToList();

                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        if (item.bprsc_qty >= obj1.Rice_Production_ProductWorth_SoldQty && item.bprsc_qty > 0)
                        {

                            decimal diff1 = item.bprsc_qty - obj1.Rice_Production_ProductWorth_SoldQty;
                            obj1.Rice_Production_ProductWorth_SoldQty -= Convert.ToInt32(obj1.Rice_Production_ProductWorth_SoldQty);
                            //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (item.bprsc_qty < obj1.Rice_Production_ProductWorth_SoldQty && item.bprsc_qty > 0)
                        {
                            decimal diff1 = item.bprsc_qty - obj1.Rice_Production_ProductWorth_SoldQty;
                            obj1.Rice_Production_ProductWorth_SoldQty -= Convert.ToInt32(item.bprsc_qty);
                            //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                    }


                }

                var Receivedtrans = db.Transaction.Where(
                      m => m.status &&
                      m.Transaction_item_id == mProducedRiceSales_pt.bprsp_id &&
                          (m.Transaction_item_type == SellingCategory.Worth_Rice_Sales || m.Transaction_item_type == SellingCategory.Worth_Rice_Sales_Remaining)
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
                trans.Transaction_Description = "Worth Return from " + db.Parties.Find(mProducedRiceSales_pt.Party_Id).Party_Name;
                trans.Transaction_item_id = prsp_id;
                trans.Transaction_item_type = SellingCategory.Worth_Rice_Sales_Return;
                trans.Debit = recieved;
                trans.Credit = 0;
                trans.status = (recieved > 0) ? true : false;
                db.Transaction.Add(trans);
                db.SaveChanges();
            }
            return Json(1, JsonRequestBehavior.AllowGet);

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

            BpRiceSales_pt ProducedRiceSales_pt_view = js.Deserialize<BpRiceSales_pt>(form["ProducedRiceSales_pt"]);
            var ProducedRiceSales_pt = db.BpRiceSales_pts.Find(ProducedRiceSales_pt_view.bprsp_id);
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
          
            ProducedRiceSales_pt.bprsp_Total_Amount = ProducedRiceSales_pt_view.bprsp_Total_Amount;
            ProducedRiceSales_pt.bprsp_Title = ProducedRiceSales_pt_view.bprsp_Title + " to Party " + db.Parties.Find(ProducedRiceSales_pt_view.Party_Id).Party_Name;
            ProducedRiceSales_pt.bprsp_status = true;
            ProducedRiceSales_pt.bprsp_date = DateTime.Now;
            db.Entry(ProducedRiceSales_pt).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            int prsp_id = ProducedRiceSales_pt.bprsp_id;
            var obj = js.DeserializeObject(form["ProducedRiceSales_ch"].ToString());
            List<BpRiceSales_ch> ProducedRiceSales_ch = js.Deserialize<BpRiceSales_ch[]>(form["ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in ProducedRiceSales_ch)
            {
                var rsc = db.BpRiceSales_chs.Find(item.bprsc_id);
                if (rsc == null)
                {
                    rsc = new BpRiceSales_ch();
                }
                
                rsc.bprsc_price = item.bprsc_price;
                rsc.bprsc_qty = item.bprsc_qty;
                rsc.bprsc_status = item.bprsc_status;
                rsc.bprsc_title = item.bprsc_title;
              
                rsc.bprsp_id = prsp_id;

                rsc.bprsc_status = true;
                if (item.bprsc_id == 0)
                {
                    db.BpRiceSales_chs.Add(rsc);
                    db.SaveChanges();


                    var Rice_Produce_Bags = db.Rice_Production_ProductWorths.Where(m => m.Rice_Production_ProductWorth_id == item.Rice_Production_ProductWorth_id).ToList();

                    decimal qty = item.bprsc_qty;

                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        decimal diff = obj1.Rice_Production_ProductWorth_Qty - obj1.Rice_Production_ProductWorth_SoldQty;
                        if (diff > qty && qty > 0)
                        {
                            obj1.Rice_Production_ProductWorth_SoldQty += Convert.ToInt32(qty);
                            //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            qty = 0;
                        }
                        else if (diff <= qty && qty > 0 && diff != 0)
                        {
                            obj1.Rice_Production_ProductWorth_SoldQty += Convert.ToInt32(diff);
                            //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
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
                        prsc_qty = newdb.BpRiceSales_chs.Where(m => m.bprsc_id == item.bprsc_id).First().bprsc_qty;
                    }
                    var Rice_Produce_Bags = db.Rice_Production_ProductWorths.Where(m => m.Rice_Production_ProductWorth_id == item.Rice_Production_ProductWorth_id).OrderByDescending(m => m.Rice_Produce_Bag.Rice_Produce_Bag_Date).ToList();

                    foreach (var obj_new in Rice_Produce_Bags)
                    {
                        if (prsc_qty >= obj_new.Rice_Production_ProductWorth_SoldQty && prsc_qty > 0)
                        {

                            decimal diff1 = prsc_qty - obj_new.Rice_Production_ProductWorth_SoldQty;
                            obj_new.Rice_Production_ProductWorth_SoldQty -= Convert.ToInt32(obj_new.Rice_Production_ProductWorth_SoldQty);
                            //obj_new.Rice_Produce_RemainingBags = obj_new.Rice_Produce_TotalBagsProduce - obj_new.Rice_Produce_BagsSold;
                            db.Entry(obj_new).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            prsc_qty = (int)diff1;
                        }
                        else if (prsc_qty < obj_new.Rice_Production_ProductWorth_SoldQty && prsc_qty > 0)
                        {
                            decimal diff1 = prsc_qty - obj_new.Rice_Production_ProductWorth_SoldQty;
                            obj_new.Rice_Production_ProductWorth_SoldQty -= Convert.ToInt32(prsc_qty);
                            //obj_new.Rice_Produce_RemainingBags = obj_new.Rice_Produce_TotalBagsProduce - obj_new.Rice_Produce_BagsSold;
                            db.Entry(obj_new).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            prsc_qty = (int)diff1;
                        }
                    }
                    db.Entry(rsc).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();




                    Rice_Produce_Bags = db.Rice_Production_ProductWorths.Where(m => m.Rice_Production_ProductWorth_id == item.Rice_Production_ProductWorth_id).ToList();

                    decimal qty = item.bprsc_qty;
                    foreach (var obj1 in Rice_Produce_Bags)
                    {
                        decimal diff = obj1.Rice_Production_ProductWorth_Qty - obj1.Rice_Production_ProductWorth_SoldQty;
                        if (diff > qty && qty > 0)
                        {
                            obj1.Rice_Production_ProductWorth_SoldQty += Convert.ToInt32(qty);
                            //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            qty = 0;
                        }
                        else if (diff <= qty && qty > 0 && diff != 0)
                        {
                            obj1.Rice_Production_ProductWorth_SoldQty += Convert.ToInt32(diff);
                            //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                            db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            qty -= diff;
                        }

                    }
                }
            }

            List<BpRiceSales_ch> Deleted_RawRice_Sales_ch = js.Deserialize<BpRiceSales_ch[]>(form["Deleted_ProducedRiceSales_ch"].ToString()).ToList();
            foreach (var item in Deleted_RawRice_Sales_ch)
            {

                var Rice_Produce_Bags = db.Rice_Production_ProductWorths.Where(m => m.Rice_Production_ProductWorth_id == item.Rice_Production_ProductWorth_id).OrderByDescending(m => m.Rice_Produce_Bag.Rice_Produce_Bag_Date).ToList();

                foreach (var obj1 in Rice_Produce_Bags)
                {
                    if (item.bprsc_qty >= obj1.Rice_Production_ProductWorth_SoldQty && item.bprsc_qty > 0)
                    {

                        decimal diff1 = item.bprsc_qty - obj1.Rice_Production_ProductWorth_SoldQty;
                        obj1.Rice_Production_ProductWorth_SoldQty -= Convert.ToInt32(obj1.Rice_Production_ProductWorth_SoldQty);
                        //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                        db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        item.bprsc_qty = (int)diff1;
                    }
                    else if (item.bprsc_qty < obj1.Rice_Production_ProductWorth_SoldQty && item.bprsc_qty > 0)
                    {
                        decimal diff1 = item.bprsc_qty - obj1.Rice_Production_ProductWorth_SoldQty;
                        obj1.Rice_Production_ProductWorth_SoldQty -= Convert.ToInt32(item.bprsc_qty);
                        //obj1.Rice_Produce_RemainingBags = obj1.Rice_Produce_TotalBagsProduce - obj1.Rice_Produce_BagsSold;
                        db.Entry(obj1).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        item.bprsc_qty = (int)diff1;
                    }

                }
                var prsc = db.BpRiceSales_chs.Find(item.bprsc_id);
                prsc.bprsc_status = false;
                db.Entry(prsc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }

            foreach (var item in db.Transaction.Where(m => m.status
                    &&
                    m.Transaction_item_id == ProducedRiceSales_pt.bprsp_id &&
                    m.Transaction_item_type == SellingCategory.Worth_Rice_Sales
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
            trans_this.Transaction_Description = ProducedRiceSales_pt.bprsp_Title;
            trans_this.Transaction_item_id = prsp_id;
            trans_this.Transaction_item_type = SellingCategory.Worth_Rice_Sales;
            trans_this.Debit = 0;
            trans_this.Credit = Recieved_Amount;
            trans_this.status = true;
            db.Transaction.Add(trans_this);
            db.SaveChanges();
        }



    }
}