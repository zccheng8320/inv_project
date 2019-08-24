using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class FactItemWithNameController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/FactItemWithName
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/FactItemWithName/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FactItemWithName
        public List<FACT_ItemWithName> Post([FromBody]FACT_ItemWithName select2Ajax)
        {
            var table1 = (from p in db.ITEM select new FACT_ItemWithName { ITEM_NO = p.ITEM_NO, ITEM_NAME = "", L_PRICE = 0, COST = 0, L_QTY = p.QTY })
               .Union(from p in db.FACTITEM where p.FACT_CODE == select2Ajax.FACT_CODE select new FACT_ItemWithName { ITEM_NO = p.ITEM_NO, ITEM_NAME = "", L_PRICE = p.L_PRICE, COST = 0, L_QTY = 0 });

            var table2 = from p in table1 group p by p.ITEM_NO into g select new FACT_ItemWithName { ITEM_NO = g.Key, ITEM_NAME = "", L_PRICE = g.Sum(m => m.L_PRICE), COST = g.Sum(m => m.COST), L_QTY = g.Sum(m => m.L_QTY) };
            var f = from p in db.ITEM join t2 in table2 on p.ITEM_NO equals t2.ITEM_NO select new FACT_ItemWithName { ITEM_NO = t2.ITEM_NO, ITEM_NAME = p.NAME1, L_PRICE = t2.L_PRICE, COST = t2.COST, L_QTY = t2.L_QTY };
            if (select2Ajax != null && select2Ajax.ITEM_NO != null)
            {
                f = from p in db.ITEM join t2 in table2 on p.ITEM_NO equals t2.ITEM_NO where p.ITEM_NO.StartsWith(select2Ajax.ITEM_NO) select new FACT_ItemWithName { ITEM_NO = t2.ITEM_NO, ITEM_NAME = p.NAME1, L_PRICE = t2.L_PRICE, COST = t2.COST, L_QTY = t2.L_QTY };
            }
            return f.Take(100).ToList();

        }

        // PUT: api/FactItemWithName/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FactItemWithName/5
        public void Delete(int id)
        {
        }
    }
}
