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
    public class PackingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Packings
        public ActionResult Index()
        {
            return View(db.Packings.ToList());
        }

        // GET: Packings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packing packing = db.Packings.Find(id);
            if (packing == null)
            {
                return HttpNotFound();
            }
            return View(packing);
        }

        // GET: Packings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Packings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Packing_Id,Packing_Type")] Packing packing)
        {
            if (ModelState.IsValid)
            {
                db.Packings.Add(packing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packing);
        }

        // GET: Packings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packing packing = db.Packings.Find(id);
            if (packing == null)
            {
                return HttpNotFound();
            }
            return View(packing);
        }

        // POST: Packings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Packing_Id,Packing_Type")] Packing packing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(packing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packing);
        }

        // GET: Packings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Packing packing = db.Packings.Find(id);
            if (packing == null)
            {
                return HttpNotFound();
            }
            return View(packing);
        }

        // POST: Packings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Packing packing = db.Packings.Find(id);
            db.Packings.Remove(packing);
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
