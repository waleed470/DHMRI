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
    public class RawRice_Sales_chController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RawRice_Sales_ch
        public IQueryable<RawRice_Sales_ch> GetRawRice_Sales_ch()
        {
            return db.RawRice_Sales_ch;
        }

        // GET: api/RawRice_Sales_ch/5
        [ResponseType(typeof(RawRice_Sales_ch))]
        public async Task<IHttpActionResult> GetRawRice_Sales_ch(int id)
        {
            RawRice_Sales_ch rawRice_Sales_ch = await db.RawRice_Sales_ch.FindAsync(id);
            if (rawRice_Sales_ch == null)
            {
                return NotFound();
            }

            return Ok(rawRice_Sales_ch);
        }

        // PUT: api/RawRice_Sales_ch/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRawRice_Sales_ch(int id, RawRice_Sales_ch rawRice_Sales_ch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rawRice_Sales_ch.rsc_id)
            {
                return BadRequest();
            }

            db.Entry(rawRice_Sales_ch).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RawRice_Sales_chExists(id))
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

        // POST: api/RawRice_Sales_ch
        [ResponseType(typeof(RawRice_Sales_ch))]
        public async Task<IHttpActionResult> PostRawRice_Sales_ch(RawRice_Sales_ch rawRice_Sales_ch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RawRice_Sales_ch.Add(rawRice_Sales_ch);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = rawRice_Sales_ch.rsc_id }, rawRice_Sales_ch);
        }

        // DELETE: api/RawRice_Sales_ch/5
        [ResponseType(typeof(RawRice_Sales_ch))]
        public async Task<IHttpActionResult> DeleteRawRice_Sales_ch(int id)
        {
            RawRice_Sales_ch rawRice_Sales_ch = await db.RawRice_Sales_ch.FindAsync(id);
            if (rawRice_Sales_ch == null)
            {
                return NotFound();
            }

            db.RawRice_Sales_ch.Remove(rawRice_Sales_ch);
            await db.SaveChangesAsync();

            return Ok(rawRice_Sales_ch);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RawRice_Sales_chExists(int id)
        {
            return db.RawRice_Sales_ch.Count(e => e.rsc_id == id) > 0;
        }
    }
}