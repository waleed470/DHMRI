using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DHMRice.Models;

namespace DHMRice.Controllers
{
    public class BrokersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Brokers
        public ActionResult Index()
        {
            return View(db.Brokers.ToList());
        }

        // GET: Brokers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Broker broker = db.Brokers.Find(id);
            if (broker == null)
            {
                return HttpNotFound();
            }
            return View(broker);
        }

        // GET: Brokers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brokers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Broker_Id,Broker_Name,Broker_Code,Broker_MobileNo,Broker_BankName,Broker_ACcountNo,Broker_Address,Status")] Broker broker)
        {
            if (ModelState.IsValid)
            {
                broker.Status = true;
                db.Brokers.Add(broker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(broker);
        }

        // GET: Brokers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Broker broker = db.Brokers.Find(id);
            if (broker == null)
            {
                return HttpNotFound();
            }
            return View(broker);
        }

        // POST: Brokers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Broker_Id,Broker_Name,Broker_Code,Broker_MobileNo,Broker_BankName,Broker_ACcountNo,Broker_Address,Status")] Broker broker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(broker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(broker);
        }

        // GET: Brokers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Broker broker = db.Brokers.Find(id);
            if (broker == null)
            {
                return HttpNotFound();
            }
            return View(broker);
        }

        // POST: Brokers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Broker broker = db.Brokers.Find(id);
            db.Brokers.Remove(broker);
            db.SaveChanges();
            return RedirectToAction("Index");
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
