using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class SalesController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/Sales
        public List<Sales> Get(string search)
        {
            var table = (from inv in db.INVOICE
                         join c in db.CUSTOMER on inv.CODE equals c.CUST_CODE
                         where inv.TRN_NO.StartsWith(search)
                         ||inv.ACC_DATE.StartsWith(search)
                         ||inv.TRN_DATE.StartsWith(search)
                         ||c.CUST_CODE.StartsWith(search)
                         || c.CUST_NAME.StartsWith(search)
                         || c.TELEPHONE.StartsWith(search)
                         select new Sales
                         {
                             TRN_NO = inv.TRN_NO,
                             ACC_DATE = inv.ACC_DATE,
                             TRN_DATE = inv.TRN_DATE,
                             CODE = c.CUST_CODE,
                             CUST_NAME = c.CUST_NAME,
                             TELEPHONE = c.TELEPHONE,
                         }).Distinct().OrderByDescending(m=>m.TRN_DATE).Take(200).ToList();
            return table;
        }

        // GET: api/Sales/5
        public List<Sales> Get(string TRN_NO, int preOrNext)
        {
            var nTRN_NO = TRN_NO;
            var id = db.INVOICE.Where(m => m.TRN_NO == TRN_NO).FirstOrDefault().ID;
            if (preOrNext == 0)
            {
                var s = (from d in db.INVOICE where d.ID<id select d).OrderByDescending(d => d.ID).FirstOrDefault();
                if(s!=null)
                    nTRN_NO = s.TRN_NO;
                else
                    nTRN_NO = "";
            }
            else
            {
                var s = (from d in db.INVOICE where d.ID>id select d).FirstOrDefault();
                if (s != null)
                    nTRN_NO = s.TRN_NO;
                else
                    nTRN_NO = "";
            }           
            var table = (from inv in db.INVOICE
                         join itran in db.ITRANS on inv.TRN_NO equals itran.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.CUSTOMER on t1.CODE equals c.CUST_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join e in db.EMPLOY on t1.SAL_NO equals e.SAL_NO into table3
                         from t3 in table3.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where inv.TRN_NO == nTRN_NO
                         select new Sales
                         {
                             TRN_NO = inv.TRN_NO,
                             INV_NO = inv.INV_NO,
                             TRN_DATE = inv.TRN_DATE,
                             CODE = inv.CODE,
                             CUST_NAME = t2.CUST_NAME,
                             TELEPHONE = t2.TELEPHONE,
                             UNIFORM = t2.UNIFORM,
                             ATTENTION = t2.ATTENTION,
                             TAX = inv.TAX,
                             REMARK1 = inv.REMARK1,
                             ACC_DATE = inv.ACC_DATE,
                             PAY_WAY = inv.PAY_WAY,
                             SUMAMT = inv.SUMAMT,
                             TAXAMT = inv.TAXAMT,
                             TOTAMT = inv.TOTAMT,
                             SAL_NO = inv.SAL_NO,
                             SAL_NAME = t3.NAME,
                             ITEM_NO = t1.ITEM_NO,
                             ITEM_NAME = t4.NAME1,
                             QTY = t1.QTY,
                             PRICE = t1.PRICE,
                             AMOUNT = t1.AMOUNT,
                             ACC_YN = inv.ACC_YN,
                             ORD_NO = t1.ORD_NO,
                             SAL_CODE = t1.SAL_CODE,
                             COST = t4.C_PRICE
                         }).ToList();
            return table;
        }

        // POST: api/Sales
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Sales/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Sales/5
        public void Delete(int id)
        {
        }
    }
}
