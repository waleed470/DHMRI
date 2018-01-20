using DHMRice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DHMRice.Controllers
{
    public class TestController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Departments
        public IQueryable<Shop> GetDepartment()
        {
            return db.Shopes;
        }

    }
}
