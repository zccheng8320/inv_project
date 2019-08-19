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
        public void Post([FromBody]List<Sales> salesList)
        {
            // 取得民國日期
            var year = DateTime.Now.Year - 1911;
            var month = "" + DateTime.Now.Month;
            var day = "" + DateTime.Now.Day;
            if (DateTime.Now.Month < 10)
            { month = "0" + month; }
            if (DateTime.Now.Day < 10)
            { day = "0" + day; }
            var today = year + "." + month + "." + day;
            var ym = year + "." + month;
            // 第一筆資料
            var sf = salesList.FirstOrDefault();           
            var invoice = db.INVOICE;
            var itrans = db.ITRANS;
            // 應收款([RECMON]) 新增或更新
            var recmon = db.RECMON.Where(m => m.CUST_CODE == sf.CODE && m.MON_DATE==sf.ACC_DATE).FirstOrDefault();
            if (recmon != null)
            {
                recmon.SAL_AMT = (Convert.ToDouble(recmon.SAL_AMT) + sf.SUMAMT).ToString();
                recmon.TAX_AMT = (Convert.ToDouble(recmon.TAX_AMT) + sf.TAXAMT).ToString();
                recmon.TOT_AMT = (Convert.ToDouble(recmon.TOT_AMT) + sf.TOTAMT).ToString();
                recmon.AMO1 = (Convert.ToDouble(recmon.AMO1) + sf.AMO1).ToString();
                recmon.AMO2 = (Convert.ToDouble(recmon.AMO2) + sf.AMO2).ToString();
                recmon.AMO3 = (Convert.ToDouble(recmon.AMO3) + sf.AMO3).ToString();
            }
            else
                db.RECMON.Add(new RECMON {CUST_CODE=sf.CODE,MON_DATE=sf.ACC_DATE ,SAL_AMT=sf.SUMAMT.ToString(),TAX_AMT=sf.TAXAMT.ToString(),TOT_AMT=sf.TOTAMT.ToString(),AMO1=sf.AMO1.ToString(), AMO2 = sf.AMO2.ToString(), AMO3 = sf.AMO3.ToString() });          
            // Customer TRN_DATE Update
            var customer = db.CUSTOMER.Where(m => m.CUST_CODE == sf.CODE).FirstOrDefault();
            customer.TRN_DATE = today;
            // Invoice Insert
            invoice.Add(new INVOICE {TRN_NO=sf.TRN_NO,INV_NO=sf.INV_NO,TRN_DATE=sf.TRN_DATE,CODE=sf.CODE,TAX=sf.TAX,REMARK1=sf.REMARK1,ACC_DATE=sf.ACC_DATE ,SUMAMT=sf.SUMAMT,TAXAMT=sf.TAXAMT,TOTAMT=sf.TOTAMT,AMO1=sf.AMO1,AMO2=sf.AMO2,AMO3=sf.AMO3,SAL_NO=sf.SAL_NO,PAY_WAY=sf.PAY_WAY,ACC_YN=sf.ACC_YN});
            // 
            foreach (var s in salesList)
            {
                // Item Update
                var item = db.ITEM.Where(m => m.ITEM_NO == s.ITEM_NO).FirstOrDefault();
                // 減去庫存量
                var n_QTY = item.QTY - Convert.ToDouble(s.QTY);
                item.QTY = n_QTY;
                item.TRN_DATE = today;
                // 更新歷史成交價
                var custitem = db.CUSTITEM.Where(m => m.CUST_CODE == s.CODE && m.ITEM_NO == s.ITEM_NO).FirstOrDefault();
                if (custitem != null)
                {
                    custitem.L_PRICE = s.PRICE;
                    custitem.L_DATE = s.TRN_DATE;
                    custitem.L_QTY = Convert.ToDouble(s.QTY);
                    custitem.SAL_CODE = s.SAL_CODE;
                    custitem.COST = item.C_PRICE;
                    custitem.REMARK = s.ORD_NO;

                }
                else
                    db.CUSTITEM.Add(new CUSTITEM { CUST_CODE = s.CODE, ITEM_NO = s.ITEM_NO,L_QTY= Convert.ToDouble(s.QTY), SAL_CODE = s.SAL_CODE, L_DATE = s.TRN_DATE, L_PRICE = s.PRICE, COST = item.C_PRICE,REMARK=s.ORD_NO });                  
                itrans.Add(new ITRANS { TRN_NO = s.TRN_NO, ITEM_NO = s.ITEM_NO,QTY=s.QTY,PRICE=s.PRICE,AMOUNT=s.AMOUNT,CODE=s.CODE,ACC_DATE=s.ACC_DATE,ACC_YN=s.ACC_YN,TRN_DATE=s.TRN_DATE,ORD_NO=s.ORD_NO,SAL_NO=s.SAL_NO,SAL_CODE=s.SAL_CODE,COST=item.C_PRICE });
            }
            db.SaveChanges();
        }

        // PUT: api/Sales/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        // DELETE: api/Sales/5
        public void Delete([FromBody]Sales sales)
        {
            
        }
    }
}
