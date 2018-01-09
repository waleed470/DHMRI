using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DHMRice.Models;

namespace DHMRice.Controllers
{
    public class RawRice_Sales_ptController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RawRice_Sales_pt
        public IQueryable<RawRice_Sales_pt> GetRawRice_Sales_pt()
        {
            return db.RawRice_Sales_pt;
        }

        // GET: api/RawRice_Sales_pt/5
        [ResponseType(typeof(RawRice_Sales_pt))]
        public async Task<IHttpActionResult> GetRawRice_Sales_pt(int id)
        {
            RawRice_Sales_pt rawRice_Sales_pt = await db.RawRice_Sales_pt.FindAsync(id);
            if (rawRice_Sales_pt == null)
            {
                return NotFound();
            }

            return Ok(rawRice_Sales_pt);
        }

        // PUT: api/RawRice_Sales_pt/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRawRice_Sales_pt(int id, RawRice_Sales_pt rawRice_Sales_pt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rawRice_Sales_pt.rsp_id)
            {
                return BadRequest();
            }

            db.Entry(rawRice_Sales_pt).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RawRice_Sales_ptExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RawRice_Sales_pt
        [ResponseType(typeof(RawRice_Sales_pt))]
        public async Task<IHttpActionResult> PostRawRice_Sales_pt(RawRice_Sales_pt rawRice_Sales_pt,decimal RecievedAmount, bool isCash,bool isBankAccount, bool isCheckbook,int CheckBookNo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RawRice_Sales_pt.Add(rawRice_Sales_pt);

            await db.SaveChangesAsync();
            Transaction trans = new Transaction();

            return CreatedAtRoute("DefaultApi", new { id = rawRice_Sales_pt.rsp_id }, rawRice_Sales_pt);
        }

        // DELETE: api/RawRice_Sales_pt/5
        [ResponseType(typeof(RawRice_Sales_pt))]
        public async Task<IHttpActionResult> DeleteRawRice_Sales_pt(int id)
        {
            RawRice_Sales_pt rawRice_Sales_pt = await db.RawRice_Sales_pt.FindAsync(id);
            if (rawRice_Sales_pt == null)
            {
                return NotFound();
            }

            db.RawRice_Sales_pt.Remove(rawRice_Sales_pt);
            await db.SaveChangesAsync();

            return Ok(rawRice_Sales_pt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RawRice_Sales_ptExists(int id)
        {
            return db.RawRice_Sales_pt.Count(e => e.rsp_id == id) > 0;
        }
    }
}