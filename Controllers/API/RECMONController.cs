using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class RECMONController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/RECMON
        public string Get(string CUST_CODE,string MON_DATE)
        {
            var TOT_AMT = db.RECMON.Where(m => m.CUST_CODE == CUST_CODE && m.MON_DATE == MON_DATE).DefaultIfEmpty().FirstOrDefault();
            if (TOT_AMT == null)
                return "0";
            else
                return TOT_AMT.TOT_AMT;
        }

        // GET: api/RECMON/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/RECMON
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/RECMON/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RECMON/5
        public void Delete(int id)
        {
        }
    }
}
