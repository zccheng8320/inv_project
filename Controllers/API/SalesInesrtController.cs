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
        // GET: api/SalesInesrt 號碼控制
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
        // 搜尋用的API      
        // POST: api/SalesInesrt
        public List<Sales> Post([FromBody]Sales s)
        {
            var table = (from inv in db.INVOICE
                         join c in db.CUSTOMER on inv.CODE equals c.CUST_CODE
                         select new Sales
                         {
                             TRN_NO = inv.TRN_NO,
                             ACC_DATE = inv.ACC_DATE,
                             TRN_DATE = inv.TRN_DATE,
                             CODE = c.CUST_CODE,
                             CUST_NAME = c.CUST_NAME,
                             TELEPHONE = c.TELEPHONE,
                         }).Distinct().OrderByDescending(m => m.TRN_DATE).Take(200).ToList();
            if (s != null && s.TRN_NO != null)
            {
                table = (from inv in db.INVOICE
                         join c in db.CUSTOMER on inv.CODE equals c.CUST_CODE
                         where inv.TRN_NO.StartsWith(s.TRN_NO)
                         || inv.ACC_DATE.StartsWith(s.TRN_NO)
                         || inv.TRN_DATE.StartsWith(s.TRN_NO)
                         || c.CUST_CODE.StartsWith(s.TRN_NO)
                         || c.CUST_NAME.StartsWith(s.TRN_NO)
                         || c.TELEPHONE.StartsWith(s.TRN_NO)
                         select new Sales
                         {
                             TRN_NO = inv.TRN_NO,
                             ACC_DATE = inv.ACC_DATE,
                             TRN_DATE = inv.TRN_DATE,
                             CODE = c.CUST_CODE,
                             CUST_NAME = c.CUST_NAME,
                             TELEPHONE = c.TELEPHONE,
                         }).Distinct().OrderByDescending(m => m.TRN_DATE).Take(200).ToList();
            }
            return table;
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
