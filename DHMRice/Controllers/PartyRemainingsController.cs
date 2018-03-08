using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DHMRice.Models;
using Newtonsoft.Json;

namespace DHMRice.Controllers
{
    public class PartyRemainingsController : Controller
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: PartyRemainings
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetParties()
        {
            var obj = db.Parties.Where(m => m.Status).ToList();            
            return Json(JsonConvert.SerializeObject(obj), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            var obj = db.PartyRemaining.ToList();
            return Json(JsonConvert.SerializeObject(obj), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert(FormCollection form)
        {
            PartyRemaining partyRemaining = js.Deserialize<PartyRemaining>(form["PartyRemaining"]);
            if (partyRemaining != null)
            {
                db.PartyRemaining.Add(partyRemaining);
                db.SaveChanges();
                return Json( db.PartyRemaining.Max(m => m.PartyRemaining_Id), JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPut]
        public void Update(FormCollection form)
        {
            PartyRemaining partyRemaining = js.Deserialize<PartyRemaining>(form["PartyRemaining"]);
            if (partyRemaining != null)
            {
                db.Entry(partyRemaining).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}