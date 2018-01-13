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
    public class ShopsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Shops
        public ActionResult Index()
        {
            return View(db.Shopes.ToList());
        }

        // GET: Shops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shopes.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // GET: Shops/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Shop_Id,Shop_Name,Shop_Code,Shop_MobileNo,Shop_BankName,Shop_ACcountNo,Shop_Address,Status")] Shop shop, List<Shop_Account> Shop_Account)
        {
            if (ModelState.IsValid)
            {
                string idd = Convert.ToString(Session["UserId"]);
                shop.Id = idd;
                shop.Status = true;
                db.Shopes.Add(shop);
                db.SaveChanges();

                var shop_id = db.Shopes.Max(m => m.Shop_Id);

                foreach (var item in Shop_Account)
                {
                    item.Shop_Id = shop_id;
                    item.Status = true;
                    db.Shop_Accounts.Add(item);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(shop);
        }

        // GET: Shops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shopes.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // POST: Shops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Shop_Id,Shop_Name,Shop_Code,Shop_MobileNo,Shop_BankName,Shop_ACcountNo,Shop_Address,Status")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shop);
        }

        // GET: Shops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shop shop = db.Shopes.Find(id);
            if (shop == null)
            {
                return HttpNotFound();
            }
            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shop shop = db.Shopes.Find(id);
            db.Shopes.Remove(shop);
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
