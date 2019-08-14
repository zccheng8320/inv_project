using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class SalesInesrtController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/SalesInesrt
        public INVOICE Get(string TRN_DATE)
        {
            // 拆解日期
            var code_ary = TRN_DATE.Split('.');
            var search_string = code_ary[0] + code_ary[1];
            var p = db.INVOICE.Where(m => m.TRN_NO.StartsWith(search_string)).OrderByDescending(m=>m.ID).FirstOrDefault();
            return p;
        }

        // GET: api/SalesInesrt/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SalesInesrt
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SalesInesrt/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SalesInesrt/5
        public void Delete(int id)
        {
        }
    }
}
