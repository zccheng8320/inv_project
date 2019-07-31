using System;
using System.Collections.Generic;
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

            var p = db.CUSTOMER.Where(m => 
            m.CUST_CODE.Contains(search)
            ||m.CUST_NAME.Contains(search)
            || m.ADDRESS1.Contains(search)
            || m.UNIFORM.Contains(search)
            || m.TELEPHONE.Contains(search)
            || m.FAX.Contains(search)
            || m.TRN_DATE.Contains(search));
            return p.ToList();
        }

        // GET: api/CUSTOMER/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CUSTOMER
        public void Post([FromBody]string value)
        {
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
