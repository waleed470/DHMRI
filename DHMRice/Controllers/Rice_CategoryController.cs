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
    public class Rice_CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rice_Category
        public ActionResult Index()
        {
            return View(db.Rice_Categories.ToList());
        }

        // GET: Rice_Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rice_Category rice_Category = db.Rice_Categories.Find(id);
            if (rice_Category == null)
            {
                return HttpNotFound();
            }
            return View(rice_Category);
        }

        // GET: Rice_Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rice_Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rice_category_Id,Rice_Category_Name,Status")] Rice_Category rice_Category)
        {
            if (ModelState.IsValid)
            {
                rice_Category.Status = true;
                db.Rice_Categories.Add(rice_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rice_Category);
        }

        // GET: Rice_Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rice_Category rice_Category = db.Rice_Categories.Find(id);
            if (rice_Category == null)
            {
                return HttpNotFound();
            }
            return View(rice_Category);
        }

        // POST: Rice_Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rice_category_Id,Rice_Category_Name,Status")] Rice_Category rice_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rice_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rice_Category);
        }

        // GET: Rice_Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rice_Category rice_Category = db.Rice_Categories.Find(id);
            if (rice_Category == null)
            {
                return HttpNotFound();
            }
            return View(rice_Category);
        }

        // POST: Rice_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rice_Category rice_Category = db.Rice_Categories.Find(id);
            db.Rice_Categories.Remove(rice_Category);
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
