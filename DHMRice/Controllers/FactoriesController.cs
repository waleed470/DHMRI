﻿using System;
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
    public class FactoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Factories
        public ActionResult Index()
        {
            return View(db.Factories.ToList());
        }

        // GET: Factories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factory factory = db.Factories.Find(id);
            if (factory == null)
            {
                return HttpNotFound();
            }
            return View(factory);
        }

        // GET: Factories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Factories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Factory_Id,Factory_Name,Factory_Code,Factory_Number,Factory_BankName,Factory_ACcountNo,Factory_Address,Status")] Factory factory)
        {
            if (ModelState.IsValid)
            {

                string idd = Convert.ToString(Session["UserId"]);
                factory.Id = idd;
                factory.Status = true;
                db.Factories.Add(factory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(factory);
        }

        // GET: Factories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factory factory = db.Factories.Find(id);
            if (factory == null)
            {
                return HttpNotFound();
            }
            return View(factory);
        }

        // POST: Factories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Factory_Id,Factory_Name,Factory_Code,Factory_Number,Factory_BankName,Factory_ACcountNo,Factory_Address,Status")] Factory factory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(factory);
        }

        // GET: Factories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factory factory = db.Factories.Find(id);
            if (factory == null)
            {
                return HttpNotFound();
            }
            return View(factory);
        }

        // POST: Factories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factory factory = db.Factories.Find(id);
            db.Factories.Remove(factory);
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
