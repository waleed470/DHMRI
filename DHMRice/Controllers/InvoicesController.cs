using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class InvoicesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Invoices
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RawRiceSellingInvoice(int? id, int In)
        {
            RawRice_Sales_pt pt = db.RawRice_Sales_pt.Find(id);
            ViewBag.InoviceNum = In;
            ViewBag.Date = pt.rsp_date;
            return View(pt);
        }

        public ActionResult ProRiceReport(int? id)
        {
            Rice_Production pt = db.Rice_Productions.Find(id);
          
            ViewBag.Date = pt.Rice_Production_Date;
            return View(pt);
        }


        public ActionResult ProRiceSellingInvoice(int? id, int In)
        {
            ProducedRiceSales_pt pt = db.ProducedRiceSales_pt.Find(id);
            ViewBag.InoviceNum = In;
            ViewBag.Date = pt.prsp_date;
            return View(pt);
        }
        public ActionResult WorthRiceSellingInvoice(int? id, int In)
        {
            BpRiceSales_pt pt = db.BpRiceSales_pts.Find(id);
            ViewBag.InoviceNum = In;
            ViewBag.Date = pt.bprsp_date;
            return View(pt);
        }
        public ActionResult ShopRiceSellingInvoice(int? id, int In)
        {
            ShopRiceSales_pt pt = db.ShopRiceSales_pt.Find(id);
            ViewBag.InoviceNum = In;
            ViewBag.Date = pt.srsp_date;
            return View(pt);
        }
    }
}