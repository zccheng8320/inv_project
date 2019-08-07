using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class FactoryController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/Factory
        public List<FACTORY> Get(string search)
        {
            // 查詢
            var p = db.FACTORY.Where(m =>
            m.FACT_CODE.Contains(search)
            || m.FACT_NAME.Contains(search)
            || m.ADDRESS1.Contains(search)
            || m.UNIFORM.Contains(search)
            || m.TELEPHONE.Contains(search)
            || m.FAX.Contains(search)
            || m.TRN_DATE.Contains(search)).Take(200);
            return p.ToList();
        }
        // GET: api/Factory/5
        public FACTORY Get(int id, int preOrNext)
        {
            if (preOrNext == 0)
            {
                var search = (from d in db.FACTORY where d.ID < id select d).OrderByDescending(d => d.ID).FirstOrDefault();
                return search;
            }
            else
            {
                var search = (from d in db.FACTORY where d.ID > id select d).OrderBy(d => d.ID).FirstOrDefault();
                return search;
            }
        }

        // POST: api/Factory
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Factory/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Factory/5
        public void Delete(int id)
        {
        }
    }
}
