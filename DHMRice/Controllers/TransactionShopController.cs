using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class TransactionShopController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
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
        public ActionResult Add(Transaction_Shop trans,FormCollection form)
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
            trans.Transaction_Shop_DateTime = DateTime.Now;
            foreach (var item in db.Opening_ClosingDays_Shop.Where(m=>m.Shop_Id==2))
            {
                if(item.Date.ToShortDateString()==DateTime.Now.ToShortDateString()&&!item.isClosed)
                {
                    trans.Opening_ClosingDays_Shop_id = item.Opening_ClosingDays_Shop_id;
                    break;
                }
            }
            trans.Transaction_Shop_item_id = 0;            
            trans.Transaction_Shop_item_type = "Manual";
            trans.Transaction_Shop_Description = form["Transaction_Shop_Description"];
            trans.status = true;
            db.Transaction_Shop.Add(trans);
            db.SaveChanges();
            return RedirectToAction("ViewToday");
        }
        public ActionResult ViewToday()
        {
            var transactions = new List<Transaction_Shop>();
            Opening_ClosingDays_Shop openclosOBj = new Opening_ClosingDays_Shop();
            foreach (var item in db.Opening_ClosingDays_Shop.Where(m=>m.Shop_Id==2))
            {
                if (item.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) && !item.isClosed)
                {
                    openclosOBj = item;
                    break;
                }
            }
            foreach (var item in db.Transaction_Shop.ToList().Where(m=>m.Opening_ClosingDays_Shop==openclosOBj).OrderByDescending(m=>m.Transaction_Shop_DateTime))
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
            return View(db.Transaction_Shop.Where(m=>m.status&&m.Opening_ClosingDays_Shop.Shop_Id==2));
        }
        public ActionResult Edit(int? id)
        {
            var trans= db.Transaction_Shop.Find(id);
            if(trans.status)
            return View(trans);
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit(Transaction_Shop trans1,FormCollection form)
        {
            Transaction_Shop trans = db.Transaction_Shop.Find(trans1.Transaction_Shop_id);
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
            trans.Transaction_Shop_item_type =  "Manual";
            trans.Transaction_Shop_Description = trans1.Transaction_Shop_Description;
            trans.Debit = trans1.Debit;
            trans.Credit = trans1.Credit;
            db.Entry(trans).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewToday");
        }
        public ActionResult Delete(int? id)
        {
            var trans = db.Transaction_Shop.Find(id);
            trans.status = false;
            db.Entry(trans).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewToday");
        }

    }
}