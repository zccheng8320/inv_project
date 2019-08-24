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
            m.FACT_CODE.StartsWith(search)
            || m.FACT_NAME.StartsWith(search)
            || m.ADDRESS1.Contains(search)
            || m.UNIFORM.StartsWith(search)
            || m.TELEPHONE.StartsWith(search)
            || m.FAX.StartsWith(search)
            || m.TRN_DATE.StartsWith(search)).Take(200);
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
        [HttpPost]
        public List<FACTORY> Post([FromBody]FACTORY select2Ajax,int tmp=0)
        {
            var p = (from c in db.FACTORY orderby c.TRN_DATE descending select c).Take(200);
            if (tmp==1)
            {
                if (select2Ajax != null && select2Ajax.FACT_CODE != null)
                {
                    p = db.FACTORY.Where(m =>
                       m.FACT_CODE.StartsWith(select2Ajax.FACT_CODE)
                       || m.FACT_NAME.Contains(select2Ajax.FACT_CODE)
                       || m.ADDRESS1.Contains(select2Ajax.FACT_CODE)
                       || m.UNIFORM.StartsWith(select2Ajax.FACT_CODE)
                       || m.TELEPHONE.StartsWith(select2Ajax.FACT_CODE)
                       || m.TRN_DATE.StartsWith(select2Ajax.FACT_CODE)).Take(200);
                }                 
               return p.ToList();
            }
            else
            {
                if (select2Ajax != null && select2Ajax.FACT_CODE != null)
                {
                    p = from c in db.FACTORY where c.FACT_CODE.StartsWith(select2Ajax.FACT_CODE) || c.FACT_NAME.StartsWith(select2Ajax.FACT_CODE) select c;
                }
                return p.ToList();
            }           
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
