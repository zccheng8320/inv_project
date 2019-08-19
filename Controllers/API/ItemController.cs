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
    public class ItemController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/Item
       
            public List<ITEM> Get(string search)
        {

            double tmp = 777;
            try
            {
                tmp = double.Parse(search);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            // 查詢
            var p = db.ITEM.Where(m =>
            m.ITEM_NO.Contains(search)
            || m.NAME1.Contains(search)
            || m.UNIT.Contains(search)
            || m.CLASS.Contains(search)
            ||m.QTY== tmp
            ||m.S_PRICE==tmp
            ||m.U_PRICE==tmp
            || m.TRN_DATE.Contains(search)).Take(200);
            return p.ToList();
        }

        // GET: api/Item/5
        public ITEM Get(int id, int preOrNext)
        {
            if (preOrNext == 0)
            {
                var search = (from d in db.ITEM where d.ID < id select d).OrderByDescending(d => d.ID).FirstOrDefault();
                return search;
            }
            else
            {
                var search = (from d in db.ITEM where d.ID > id select d).OrderBy(d => d.ID).FirstOrDefault();
                return search;
            }
        }

        // POST: api/Item
        public List<ITEM> Post([FromBody]ITEM select2Ajax, int tmp = 0)
        {
            double temp = 777;
            try
            {
                temp = double.Parse(select2Ajax.ITEM_NO);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            // 查詢           
            var p = (from c in db.ITEM orderby c.TRN_DATE descending select c).Take(200);
            if (tmp == 1)
            {
                if (select2Ajax != null && select2Ajax.ITEM_NO != null)
                {
                     p = db.ITEM.Where(m =>
                     m.ITEM_NO.StartsWith(select2Ajax.ITEM_NO)
                     || m.NAME1.Contains(select2Ajax.ITEM_NO)).Take(200);
                }
                return p.ToList();
            }
            else
            {
                if (select2Ajax != null && select2Ajax.ITEM_NO != null)
                {
                    p = from c in db.ITEM where c.ITEM_NO.StartsWith(select2Ajax.ITEM_NO) || c.NAME1.Contains(select2Ajax.ITEM_NO) select c;
                }
                return p.ToList();
            }
        }

        // PUT: api/Item/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Item/5
        public void Delete(int id)
        {
        }
    }
}
