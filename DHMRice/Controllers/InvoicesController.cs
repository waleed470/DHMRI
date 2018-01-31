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

        public ActionResult RawRiceSellingInvoice(int? id)
        {
            RawRice_Sales_pt pt = db.RawRice_Sales_pt.Find(id);
            return View(pt);
        }
    }
}