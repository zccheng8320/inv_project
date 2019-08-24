using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class PURCHASEController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/PURCHASE
        public List<PURCHASE> Get(string TRN_NO, int preOrNext)
        {
            var nTRN_NO = TRN_NO;
            var id = db.PURCH.Where(m => m.TRN_NO == TRN_NO).FirstOrDefault().ID;
            if (preOrNext == 0)
            {
                var s = (from d in db.PURCH where d.ID < id select d).OrderByDescending(d => d.ID).FirstOrDefault();
                if (s != null)
                    nTRN_NO = s.TRN_NO;
                else
                    nTRN_NO = "";
            }
            else
            {
                var s = (from d in db.PURCH where d.ID > id select d).FirstOrDefault();
                if (s != null)
                    nTRN_NO = s.TRN_NO;
                else
                    nTRN_NO = "";
            }
            var table = (from purch in db.PURCH
                         join ptrans in db.PTRANS on purch.TRN_NO equals ptrans.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.FACTORY on purch.CODE equals c.FACT_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where purch.TRN_NO == nTRN_NO
                         select new PURCHASE
                         {
                             TRN_NO = purch.TRN_NO,
                             INV_NO = purch.INV_NO,
                             TRN_DATE = purch.TRN_DATE,
                             CODE = purch.CODE,
                             FACT_NAME = t2.FACT_NAME,
                             TELEPHONE = t2.TELEPHONE,
                             UNIFORM = t2.UNIFORM,
                             ATTENTION = t2.ATTENTION,
                             TAX = purch.TAX,
                             REMARK1 = purch.REMARK1,
                             ACC_DATE = purch.ACC_DATE,
                             PAY_WAY = purch.PAY_WAY,
                             SUMAMT = purch.SUMAMT,
                             TAXAMT = purch.TAXAMT,
                             TOTAMT = purch.TOTAMT,
                             ITEM_NO = t1.ITEM_NO,
                             ITEM_NAME = t4.NAME1,
                             QTY = t1.QTY,
                             PRICE = purch.AMO1,
                             AMOUNT = purch.AMO1,
                             ACC_YN = purch.ACC_YN,
                             ORD_NO = t1.ORD_NO,
                             COST = t4.C_PRICE.ToString(),
                             ITEM_ID = t1.ID,
                             AMO1 = purch.AMO1,
                             AMO2 = purch.AMO2,
                             AMO3 = purch.AMO3
                         }).ToList();
            return table;
        }

        // GET: api/PURCHASE/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PURCHASE
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PURCHASE/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PURCHASE/5
        public void Delete(int id)
        {
        }
    }
}
