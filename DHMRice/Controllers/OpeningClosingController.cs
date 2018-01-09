using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class OpeningClosingController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: OpeningClosing
        public ActionResult Index()
        {            
            return View(db.Opening_ClosingDays.Where(m=>m.status).ToList());
        }
        public ActionResult ViewTransactionByOpeningDay(int id)
        {
            Opening_ClosingDays opcd = db.Opening_ClosingDays.Find(id);
            if (opcd != null)
                return View( db.Transaction.Where(m => m.Opening_ClosingDays.Opening_ClosingDays_id == opcd.Opening_ClosingDays_id).ToList());
            else
                return HttpNotFound();
        }
        public JsonResult CheckIntialOpening()
        {
            var Opening_ClosingDays = db.Opening_ClosingDays.ToList();
            if ( Opening_ClosingDays.Count() == 0)
            {
                
                return Json(1, JsonRequestBehavior.AllowGet);
            }             
              
            
            bool day_det = false;
            foreach (var item in Opening_ClosingDays)
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
                Opening_ClosingDays openclosOBj = db.Opening_ClosingDays.Find(Opening_ClosingDays.Max(m => m.Opening_ClosingDays_id));
                if (openclosOBj != null && !openclosOBj.isClosed)
                {
                    Bal = openclosOBj.Opening_Balance;
                    foreach (var item in db.Transaction.ToList().Where(m => m.Opening_ClosingDays == openclosOBj))
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

                    Opening_ClosingDays newDay = new Opening_ClosingDays();
                    newDay.Closing_Balance = 0;
                    newDay.Date = DateTime.Now;
                    newDay.isClosed = false;
                    newDay.Opening_Balance = Bal;
                    newDay.status = true;
                    db.Opening_ClosingDays.Add(newDay);
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
            var list = db.Opening_ClosingDays.ToList();
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
        public ActionResult AddInitialOpenningDay(Opening_ClosingDays obj)
        {
            obj.Closing_Balance = 0;
            obj.Date = DateTime.Now;
            obj.isClosed = false;
            obj.status = true;
            db.Opening_ClosingDays.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index", "Factories");
        }
        public ActionResult ClosingdayConfirmation()
        {
            decimal Bal = 0;
            Opening_ClosingDays openclosOBj = new Opening_ClosingDays();
            foreach (var item in db.Opening_ClosingDays)
            {
                if (item.Date.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) && !item.isClosed)
                {
                    Bal = item.Opening_Balance;
                    openclosOBj = item;
                    break;
                }
            }
            foreach (var item in db.Transaction.ToList().Where(m => m.Opening_ClosingDays == openclosOBj))
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
                Opening_ClosingDays openclosOBj = null;
                foreach (var item in db.Opening_ClosingDays)
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
                    foreach (var item in db.Transaction.ToList().Where(m => m.Opening_ClosingDays == openclosOBj))
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

                    Opening_ClosingDays newDay = new Opening_ClosingDays();
                    newDay.Closing_Balance = 0;
                    newDay.Date = DateTime.Now;
                    newDay.isClosed = false;
                    newDay.Opening_Balance = Bal;
                    newDay.status = true;
                    db.Opening_ClosingDays.Add(newDay);
                    db.SaveChanges();
                }
                
            }
            return RedirectToAction("ViewToday", "Transaction");

        }
    }
    }