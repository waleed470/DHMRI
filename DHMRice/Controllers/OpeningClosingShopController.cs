using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class OpeningClosingShopController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: OpeningClosing
        public ActionResult Index()
        {            
            return View(db.Opening_ClosingDays_Shop.Where(
                m=>m.status&&
                m.Shop_Id==2).ToList());
        }
        public ActionResult ViewTransactionByOpeningDay(int id)
        {
            Opening_ClosingDays_Shop opcd = db.Opening_ClosingDays_Shop.Find(id);
            if (opcd != null)
                return View( db.Transaction_Shop.Where(
                    m => m.Opening_ClosingDays_Shop.Opening_ClosingDays_Shop_id == opcd.Opening_ClosingDays_Shop_id&&
                    m.Transaction_Shop_id==2)
                    .ToList());
            else
                return HttpNotFound();
        }
        public JsonResult CheckIntialOpening()
        {
            var Opening_ClosingDays_Shop = db.Opening_ClosingDays_Shop.Where(
                m=>m.Shop_Id == 2).ToList();
            if (Opening_ClosingDays_Shop.Count() == 0)
            {
                
                return Json(1, JsonRequestBehavior.AllowGet);
            }             
              
            
            bool day_det = false;
            foreach (var item in Opening_ClosingDays_Shop)
            {
                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                {
                    day_det = true;
                    break;
                }
            }
            if (!day_det)
            {
                decimal Bal = 0;
                Opening_ClosingDays_Shop openclosOBj = db.Opening_ClosingDays_Shop.Find(Opening_ClosingDays_Shop.Max(m => m.Opening_ClosingDays_Shop_id));
                if (openclosOBj != null && !openclosOBj.isClosed && openclosOBj.Shop_Id==2)
                {
                    Bal = openclosOBj.Opening_Balance;
                    foreach (var item in db.Transaction_Shop.ToList().Where(m => m.Opening_ClosingDays_Shop == openclosOBj))
                    {
                        if (item.status)
                        {
                            if (item.Debit > 0)
                            {
                                Bal -= item.Debit;
                            }
                            else if (item.Credit > 0)
                            {
                                Bal += item.Credit;
                            }
                        }
                    }
                    openclosOBj.Closing_Balance = Bal;
                    openclosOBj.isClosed = true;
                    db.Entry(openclosOBj).State = EntityState.Modified;
                    db.SaveChanges();

                    Opening_ClosingDays_Shop newDay = new Opening_ClosingDays_Shop();
                    newDay.Shop_Id = 2; 
                    newDay.Closing_Balance = 0;
                    newDay.Date = DateTime.Now;
                    newDay.isClosed = false;
                    newDay.Opening_Balance = Bal;
                    newDay.status = true;
                    db.Opening_ClosingDays_Shop.Add(newDay);
                    db.SaveChanges();
                }
                else
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCureentOpeningBalance()
        {
            var list = db.Opening_ClosingDays_Shop.Where(m=>m.Shop_Id==2).ToList();
            foreach (var item in list)
            {
                if (item.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) && !item.isClosed)
                {
                    return Json(item.Opening_Balance, JsonRequestBehavior.AllowGet);
                }
            }
            return null;

        }
        public ActionResult AddInitialOpenningDay()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddInitialOpenningDay(Opening_ClosingDays_Shop obj)
        {
            obj.Shop_Id = 2;                
            obj.Closing_Balance = 0;
            obj.Date = DateTime.Now;
            obj.isClosed = false;
            obj.status = true;
            db.Opening_ClosingDays_Shop.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index", "Factories");
        }
        public ActionResult ClosingdayConfirmation()
        {
            decimal Bal = 0;
            Opening_ClosingDays_Shop openclosOBj = new Opening_ClosingDays_Shop();
            foreach (var item in db.Opening_ClosingDays_Shop.Where(m=>m.Shop_Id==2))
            {
                if (item.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) && !item.isClosed)
                {
                    Bal = item.Opening_Balance;
                    openclosOBj = item;
                    break;
                }
            }
            foreach (var item in db.Transaction_Shop.ToList().Where(m => m.Opening_ClosingDays_Shop == openclosOBj))
            {
                if (item.status)
                {
                    if (item.Debit > 0)
                    {
                        Bal -= item.Debit;
                    }
                    else if (item.Credit > 0)
                    {
                        Bal += item.Credit;
                    }
                }
            }

            return View(Bal);
        }
        [HttpPost]
        public ActionResult ClosingdayConfirmation(FormCollection form)
        {
            if(form["Yes"]!=null)
            {
                decimal Bal = 0;
                Opening_ClosingDays_Shop openclosOBj = null;
                foreach (var item in db.Opening_ClosingDays_Shop.Where(m=>m.Shop_Id==2))
                {
                    if (item.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) && !item.isClosed)
                    {
                        Bal = item.Opening_Balance;
                        openclosOBj = item;
                        break;
                    }
                }
                
                if(openclosOBj!=null)
                {
                    foreach (var item in db.Transaction_Shop.ToList().Where(m => m.Opening_ClosingDays_Shop == openclosOBj))
                    {
                        if (item.status)
                        {
                            if (item.Debit > 0)
                            {
                                Bal -= item.Debit;
                            }
                            else if (item.Credit > 0)
                            {
                                Bal += item.Credit;
                            }
                        }
                    }
                    openclosOBj.Closing_Balance = Bal;
                    openclosOBj.isClosed = true;
                    db.Entry(openclosOBj).State = EntityState.Modified;
                    db.SaveChanges();

                    Opening_ClosingDays_Shop newDay = new Opening_ClosingDays_Shop();
                    newDay.Shop_Id = 2;
                    newDay.Closing_Balance = 0;
                    newDay.Date = DateTime.Now;
                    newDay.isClosed = false;
                    newDay.Opening_Balance = Bal;
                    newDay.status = true;
                    db.Opening_ClosingDays_Shop.Add(newDay);
                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("ViewToday", "Transaction");

        }
    }
    }