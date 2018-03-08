using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DHMRice.Controllers
{
    public class TransactionController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        int Opening_ClosingDays_id;
        public TransactionController()
        {
           
        }
        // GET: Transaction
        public ActionResult Index()
        {
            return RedirectToAction("ViewToday");
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Transaction trans,FormCollection form)
        {
            if (form["isBankAccount"] != null)
            {
                trans.BankAccountNo = form["BankAccNo"];
            }
            else if (form["isCheckbook"] != null)
            {
                trans.BankAccountNo = form["BankAccNo"];
                trans.checkno = Convert.ToInt32(form["CheckNo"]);
            }
            else if (form["isCash"] != null)
            {
                trans.isByCash = true;
            }
            trans.Transaction_DateTime = DateTime.Now;
            foreach (var item in db.Opening_ClosingDays)
            {
                if(item.Date.ToShortDateString()==DateTime.Now.ToShortDateString()&&!item.isClosed)
                {
                    trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                    break;
                }
            }
            trans.Transaction_item_id = 0;            
            trans.Transaction_item_type = (form["IsDriverExpense"] == "1")? "DriverExpense":"Manual";
            trans.Transaction_Description = form["Transaction_Description"];
            trans.status = true;
            db.Transaction.Add(trans);
            db.SaveChanges();
            return RedirectToAction("ViewToday");
        }
        public ActionResult ViewToday()
        {
            var transactions = new List<Transaction>();
            Opening_ClosingDays openclosOBj = new Opening_ClosingDays();
            foreach (var item in db.Opening_ClosingDays)
            {
                if (item.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) && !item.isClosed)
                {
                    openclosOBj = item;
                    break;
                }
            }
            foreach (var item in db.Transaction.ToList().Where(m=>m.Opening_ClosingDays==openclosOBj).OrderByDescending(m=>m.Transaction_DateTime))
            {
                if(item.status)
                {
                    transactions.Add(item);
                }
            }
            return View(transactions);
        }
        public ActionResult ViewAll()
        {          
            return View(db.Transaction.Where(m=>m.status));
        }
        public ActionResult Edit(int? id)
        {
            var trans= db.Transaction.Find(id);
            if(trans.status)
            return View(trans);
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit(Transaction trans1,FormCollection form)
        {
            Transaction trans = db.Transaction.Find(trans1.Transaction_id);
            if (form["isBankAccount"] != null)
            {
                trans.BankAccountNo = form["BankAccNo"];
            }
            else if (form["isCheckbook"] != null)
            {
                trans.BankAccountNo = form["BankAccNo"];
                trans.checkno = Convert.ToInt32(form["CheckNo"]);
            }
            else if (form["isCash"] != null)
            {
                trans.isByCash = true;
            }
            trans.Transaction_item_type = (form["IsDriverExpense"] == "1") ? "DriverExpense" : "Manual";
            trans.Transaction_Description = trans1.Transaction_Description;
            trans.Debit = trans1.Debit;
            trans.Credit = trans1.Credit;
            db.Entry(trans).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewToday");
        }
        public ActionResult Delete(int? id)
        {
            var trans = db.Transaction.Find(id);
            trans.status = false;
            db.Entry(trans).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewToday");
        }
        [HttpPost]
        public void JSON_Transaction(FormCollection form)
        {
            foreach (var item in db.Opening_ClosingDays)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                    break;
                }
            }
            var js = new JavaScriptSerializer();
            Transaction mTransaction = js.Deserialize<Transaction>(form["Transaction"]);
            if (mTransaction != null)
            {
                mTransaction.Transaction_DateTime = DateTime.Now;
                mTransaction.Opening_ClosingDays_id = Opening_ClosingDays_id;
                db.Transaction.Add(mTransaction);
                db.SaveChanges();
            }
        }

    }
}