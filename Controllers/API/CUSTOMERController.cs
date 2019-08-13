using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class CUSTOMERController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/CUSTOMER
        public List<CUSTOMER> Get(string search)
        {
            // 查詢
            var p = db.CUSTOMER.Where(m => 
            m.CUST_CODE.Contains(search)
            ||m.CUST_NAME.Contains(search)
            || m.ADDRESS1.Contains(search)
            || m.UNIFORM.Contains(search)
            || m.TELEPHONE.Contains(search)
            || m.FAX.Contains(search)
            || m.TRN_DATE.Contains(search)).Take(200);
            return p.ToList();
        }

        // GET: api/CUSTOMER/5

        public CUSTOMER Get(int id,int preOrNext)
        {
            if(preOrNext==0)
            {
                var search = (from d in db.CUSTOMER where d.ID < id select d).OrderByDescending(d => d.ID).FirstOrDefault();
                return search;
            }
            else
            {
                var search = (from d in db.CUSTOMER where d.ID > id select d).OrderBy(d => d.ID).FirstOrDefault();
                return search;
            }
        }


        // POST: api/CUSTOMER
        [HttpPost]
        public List<CUSTOMER> Post([FromBody]CUSTOMER select2Ajax)
        {
            
            var p = (from c in db.CUSTOMER orderby c.TRN_DATE descending select c).Take(25);
            Debug.WriteLine("string="+select2Ajax.CUST_CODE);
            if (select2Ajax != null && select2Ajax.CUST_CODE!=null)
            {
                p = from c in db.CUSTOMER where c.CUST_CODE.StartsWith(select2Ajax.CUST_CODE) || c.CUST_NAME.StartsWith(select2Ajax.CUST_CODE) select c;
            }
            return p.ToList();
        }

        // PUT: api/CUSTOMER/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CUSTOMER/5
        public void Delete(int id)
        {
        }
    }
}
