using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reports
        public ActionResult Index()
        {
            return View(db.RarRices.Where(m => m.Status).ToList());
        }

        public ActionResult AllRice()
        {

            var rawrice = TempData["RawRiceData"];

            return View(rawrice);
        }
        public ActionResult RawRicePurchasing()
        {

            var rawrice = TempData["RawRiceData"];
            ViewBag.DateFrom = TempData["DateFrom"];
            ViewBag.DateTo = TempData["DateTo"];


            return View(rawrice);
        }

        public ActionResult RawRiceSelling()
        {

            var rawrice = TempData["RawRiceData"];
            ViewBag.DateFrom = TempData["DateFrom"];
            ViewBag.DateTo = TempData["DateTo"];


            return View(rawrice);
        }


       
        public ActionResult AllRawRiceSelling()
        {

            var rawrice = TempData["RawRiceData"];
            ViewBag.DateFrom = TempData["DateFrom"];
            ViewBag.DateTo = TempData["DateTo"];


            return View(rawrice);
        }

        public ActionResult TodaySale()
        {

            var rawrice = TempData["Todaysale"];
           


            return View(rawrice);
        }

        public ActionResult RawPurchase(int ReportType, int Month, int Invoice, int? DateFrom, int? DateTo, int? RawRice_id)
        {
            if(Invoice == 1)
            {
                if (ReportType == 1)
                {
                    if (DateFrom == null)
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month).ToList();
                        TempData["RawRiceData"] = rawrice;
                    }else
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.Date.Day >= DateFrom && r.Date.Day <= DateTo).ToList();
                        TempData["RawRiceData"] = rawrice;
                    }
                   
                    return RedirectToAction("AllRice");
                }
                if (ReportType == 2)
                {

                    var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.Date.Day>= DateFrom &&r.Date.Day<=DateTo).ToList();
                    TempData["RawRiceData"] = rawrice;
                    return RedirectToAction("AllRice");
                }
            }
            if (Invoice == 2)
            {
                if (RawRice_id != null)
                {
                    if (ReportType == 1 && DateFrom ==null)
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.RawRice_id == RawRice_id).ToList();
                        TempData["RawRiceData"] = rawrice;
                        return RedirectToAction("RawRiceSelling");
                    }
                    if (ReportType == 2)
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.RawRice_id == RawRice_id && r.Date.Day >= DateFrom && r.Date.Day <= DateTo).ToList();
                        TempData["RawRiceData"] = rawrice;
                        
                        TempData["DateFrom"] = DateFrom;
                        TempData["DateTo"] = DateTo;
                        return RedirectToAction("RawRiceSelling");
                    }
                    //if (ReportType == 2)
                    //{

                    //    var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.Date.Day >= DateFrom && r.Date.Day <= DateTo).ToList();
                    //    TempData["RawRiceData"] = rawrice;
                    //    return RedirectToAction("AllRice");
                    //}
                }
                else {
                    if (ReportType == 1)
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month ).ToList();
                        TempData["RawRiceData"] = rawrice;
                        return RedirectToAction("AllRawRiceSelling");
                    }
                    if (ReportType == 2)
                    {

                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.Date.Day >= DateFrom && r.Date.Day <= DateTo).ToList();
                        TempData["RawRiceData"] = rawrice;
                        TempData["DateFrom"] = DateFrom;
                        TempData["DateTo"] = DateTo;
                        return RedirectToAction("AllRawRiceSelling");
                    }
                }


                
            }
            if (Invoice == 3)
            {
                if (RawRice_id != null)
                {
                    if (ReportType == 1 && DateFrom == null)
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.RawRice_id == RawRice_id).ToList();
                        TempData["RawRiceData"] = rawrice;
                        return RedirectToAction("RawRicePurchasing");
                    }
                    if (ReportType == 2)
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.RawRice_id == RawRice_id && r.Date.Day >= DateFrom && r.Date.Day <= DateTo).ToList();
                        TempData["RawRiceData"] = rawrice;
                        TempData["DateFrom"] = DateFrom;
                        TempData["DateTo"] = DateTo;
                        return RedirectToAction("RawRicePurchasing");
                    }
                    //if (ReportType == 2)
                    //{

                    //    var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.Date.Day >= DateFrom && r.Date.Day <= DateTo).ToList();
                    //    TempData["RawRiceData"] = rawrice;
                    //    return RedirectToAction("AllRice");
                    //}
                }
                else
                {
                    if (ReportType == 1)
                    {
                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month).ToList();
                        TempData["RawRiceData"] = rawrice;
                        return RedirectToAction("RawRicePurchasing");
                    }
                    if (ReportType == 2)
                    {

                        var rawrice = db.RarRices.Where(r => r.Date.Month == Month && r.Date.Day >= DateFrom && r.Date.Day <= DateTo).ToList();
                        TempData["RawRiceData"] = rawrice;
                        TempData["DateFrom"] = DateFrom;
                        TempData["DateTo"] = DateTo;
                        return RedirectToAction("RawRicePurchasing");
                    }
                }



            }


            return View();
           
        }

        public ActionResult DailyReport(int? ReportType, int? Month, int Invoice, int? DateFrom, int? DateTo)
        {
            if (Invoice == 1)
            {
                int Date = DateTime.Now.Day;
                int Months = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                var Todaysale = db.Transaction.Where(r => r.Credit>0 && r.Transaction_DateTime.Day==Date && r.Transaction_DateTime.Month==Months && r.Transaction_DateTime.Year==year).ToList();
                TempData["Todaysale"] = Todaysale;
                return RedirectToAction("TodaySale");
            }
            if (Invoice == 2)
            {
                int Date = DateTime.Now.Day;
                int Months = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                var Todaysale = db.Transaction.Where(r => r.Debit > 0 && r.Transaction_DateTime.Day == Date && r.Transaction_DateTime.Month == Months && r.Transaction_DateTime.Year == year).ToList();
                TempData["Todaysale"] = Todaysale;
                return RedirectToAction("TodaySale");
            }
            if (Invoice == 3)
            {
                int Date = DateTime.Now.Day;
                int Months = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                var Todaysale = db.Transaction.Where(r => r.Debit > 0 &&  r.Transaction_DateTime.Month == Month && r.Transaction_DateTime.Day >= DateFrom && r.Transaction_DateTime.Day <= DateTo).ToList();
                TempData["Todaysale"] = Todaysale;
                return RedirectToAction("TodaySale");
            }
            if (Invoice == 4)
            {
                int Date = DateTime.Now.Day;
                int Months = DateTime.Now.Month;
                int year = DateTime.Now.Year;
                var Todaysale = db.Transaction.Where(r => r.Credit > 0 && r.Transaction_DateTime.Month == Month && r.Transaction_DateTime.Day >= DateFrom && r.Transaction_DateTime.Day <= DateTo).ToList();
                TempData["Todaysale"] = Todaysale;
                return RedirectToAction("TodaySale");
            }
          
            return View();

        }

        public ActionResult RawStock()
        {

            var Rawstock = TempData["Rawstock"];



            return View(Rawstock);
        }
        public ActionResult ProducedRiceStock()
        {

            var ProducedRiceStock = TempData["ProducedRiceStock"];



            return View(ProducedRiceStock);
        }

        public ActionResult Stock( int Invoice)
        {
            if (Invoice == 1)
            {
               
                var Rawstock = db.RarRices.ToList();
                TempData["Rawstock"] = Rawstock;
                return RedirectToAction("RawStock");
            }
            if (Invoice == 2)
            {
               
                var ProducedStock = db.Rice_Productions.ToList();
                TempData["ProducedStock"] = ProducedStock;
                return RedirectToAction("ProducedRiceStock");
            }
            

            return View();

        }
        public ActionResult PartyDetail(int Party_Id)
        {

            var parties = db.Parties.Where(p => p.Party_Id == Party_Id).SingleOrDefault();


            return View(parties);

        }
    }
}
