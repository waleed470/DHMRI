using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DHMRice.Controllers
{
    public class RawRicePartyRemainingController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: RawRicePartyRemaining
        public ActionResult Index()
        {
            var parties = db.Parties.Where(m => m.Status).ToList();
            return View(parties);
        }
        [HttpPost]
        public ActionResult Index(List<RawRice> RawRice_Remaining, List<decimal> Previous_Remainings, List<int> RawRice_Remaining_checkbox, int Party_Id,FormCollection form)
        {
            if (RawRice_Remaining != null && Previous_Remainings != null && RawRice_Remaining_checkbox != null)
            {
                for (int i = 0; i < RawRice_Remaining.Count; i++)
                {
                    try
                    {
                        if (RawRice_Remaining_checkbox.Where(m => m.Equals(RawRice_Remaining[i].RawRice_id)).Count() > 0)
                        {
                            Transaction rem_trans = new Transaction();
                            rem_trans.Transaction_item_id = RawRice_Remaining[i].RawRice_id;
                            rem_trans.Transaction_item_type = "RawRice";
                            rem_trans.Transaction_Description = "Pay Remaining Amount of " + RawRice_Remaining[i].Item_Name;
                            rem_trans.Transaction_DateTime = DateTime.Now;
                            if (form["isBankAccount"] != null)
                            {
                                rem_trans.BankAccountNo = form["BankAccountNo"];
                            }
                            else if (form["isCheckbook"] != null)
                            {
                                rem_trans.checkno = Convert.ToInt32(form["CheckNo"]);
                                rem_trans.BankAccountNo = form["BankAccountNo"];
                            }
                            else if (form["isCash"] != null)
                            {
                                rem_trans.isByCash = true;
                                rem_trans.BankAccountNo = "";
                            }
                            foreach (var item in db.Opening_ClosingDays)
                            {
                                if (item.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && !item.isClosed)
                                {
                                    rem_trans.Opening_ClosingDays_id = item.Opening_ClosingDays_id;
                                    break;
                                }
                            }
                            rem_trans.Debit = Previous_Remainings[i];
                            rem_trans.Credit = 0;
                            rem_trans.status = true;
                            db.Transaction.Add(rem_trans);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            var parties = db.Parties.Where(m => m.Status).ToList();
            return View(parties);
        }
    }
}