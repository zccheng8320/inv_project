using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using INV_Project.Models;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace INV_Project.Controllers
{
    public class PSIController : Controller
    {
        private invEntities db = new invEntities();
        private string CookieID = "TRN_NO";
        private string fTRN_NO = "";   

        // GET: PSI
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sales(string TRN_NO= "")
        {
            fTRN_NO = db.INVOICE.FirstOrDefault().TRN_NO;
            try
            {
                TRN_NO = Request.Cookies[CookieID].Value.ToString();
            }
            catch (Exception e)
            {
                TRN_NO = "";
            }
            var table = (from inv in db.INVOICE
                         join itran in db.ITRANS on inv.TRN_NO equals itran.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.CUSTOMER on inv.CODE equals c.CUST_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join e in db.EMPLOY on inv.SAL_NO equals e.SAL_NO into table3
                         from t3 in table3.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where inv.TRN_NO == TRN_NO
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
                             SAL_CODE=t1.SAL_CODE,
                             COST = t4.C_PRICE
                         }).ToList();
            if (TRN_NO != "")
            {            
                HttpCookie Cookie = new HttpCookie(CookieID, table.FirstOrDefault().TRN_NO);
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                table = (from inv in db.INVOICE
                         join itran in db.ITRANS on inv.TRN_NO equals itran.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.CUSTOMER on inv.CODE equals c.CUST_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join e in db.EMPLOY on inv.SAL_NO equals e.SAL_NO into table3
                         from t3 in table3.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where inv.TRN_NO == fTRN_NO
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
                             ORD_NO = t1.ORD_NO

                         }).ToList();
                HttpCookie Cookie = new HttpCookie(CookieID, table.FirstOrDefault().TRN_NO);
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }                     
            return View(table);
        }
        public ActionResult Search(string TRN_NO = "")
        {
            fTRN_NO = db.INVOICE.FirstOrDefault().TRN_NO;
            if(TRN_NO=="")
                TRN_NO = fTRN_NO;
            var id = db.INVOICE.Where(m => m.TRN_NO == TRN_NO).FirstOrDefault().ID;
            var table = (from inv in db.INVOICE                         
                         join c in db.CUSTOMER on inv.CODE equals c.CUST_CODE              
                         where inv.ID>=id
                         select new Sales
                         {
                             TRN_NO = inv.TRN_NO,
                             ACC_DATE = inv.ACC_DATE,
                             TRN_DATE = inv.TRN_DATE,
                             CODE = c.CUST_CODE,
                             CUST_NAME = c.CUST_NAME,
                             TELEPHONE = c.TELEPHONE,                                                                                                        
                         }).Distinct().Take(200).ToList();
            return View(table);
        }
        public ActionResult Insert()
        {
            return View();
        }
        public ActionResult Update(string TRN_NO = "")
        {
            var table = (from inv in db.INVOICE
                         join itran in db.ITRANS on inv.TRN_NO equals itran.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.CUSTOMER on inv.CODE equals c.CUST_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join e in db.EMPLOY on inv.SAL_NO equals e.SAL_NO into table3
                         from t3 in table3.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where inv.TRN_NO == TRN_NO
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
                             COST = t4.C_PRICE,
                             AMO1=inv.AMO1,
                             AMO2 = inv.AMO2,
                             AMO3 = inv.AMO3,
                             ITEM_ID=t1.ID
                         }).ToList();
            return View(table);
        }
        [HttpPost]
        public void Update_Sales([FromBody]List<Sales> salesList)
        {
            var SF = salesList.FirstOrDefault();
            var invoice = db.INVOICE.Where(m => m.TRN_NO == SF.TRN_NO).DefaultIfEmpty().FirstOrDefault();
            var new_inv = SalesConvertInvoice(SF);
            invoice = new_inv;
            db.SaveChanges();
            foreach(var s in salesList)
            {
                if(s.ITEM_ID!=null)
                {
                    var itrans = db.ITRANS.Where(m => m.ID == s.ITEM_ID).FirstOrDefault();
                    var new_itrans = SalesConvertITRANS(s);
                    if (itrans != null)
                        itrans = new_itrans;
                    else
                        db.ITRANS.Add(new_itrans);
                    db.SaveChanges();
                }
            }
        }
        [HttpPost]
        public void Delete_Item([FromBody]Sales sales)
        {
            Debug.WriteLine("sales="+sales.TRN_NO);
            var itrans = db.ITRANS.Where(m => m.ID == sales.ITEM_ID).FirstOrDefault();
            db.ITRANS.Remove(itrans);
            var invoice = db.INVOICE.Where(m => m.TRN_NO == sales.TRN_NO).FirstOrDefault();
            invoice.TAX = sales.TAX; invoice.REMARK1 = sales.REMARK1;
            invoice.PAY_WAY = sales.PAY_WAY; invoice.SUMAMT = sales.SUMAMT;
            invoice.TAXAMT = sales.TAXAMT;
            invoice.AMO1 = sales.AMO1; invoice.TAX = sales.TAX;
            invoice.TOTAMT = sales.TOTAMT; invoice.SAL_NO = sales.SAL_NO;
            invoice.AMO2 = sales.AMO2; invoice.AMO3 = sales.AMO3;
            invoice.INV_NO = sales.INV_NO;
            db.SaveChanges();

            var recmon = db.RECMON.Where(m => m.CUST_CODE == sales.CODE && m.MON_DATE == sales.ACC_DATE).FirstOrDefault();
            var in_list = db.INVOICE.Where(m => m.CODE == sales.CODE && m.ACC_DATE == sales.ACC_DATE).ToList();
            var SAL_AMT = 0.0; var TAX_AMT = 0.0; var AMO1 = 0.0; var AMO2 = 0.0; var AMO3 = 0.0;
            foreach (var i in in_list)
            {
                SAL_AMT += Convert.ToDouble(i.SUMAMT);
                TAX_AMT += Convert.ToDouble(i.TAXAMT);
                AMO1 += Convert.ToDouble(i.AMO1);
                AMO2 += Convert.ToDouble(i.AMO2);
                AMO3 += Convert.ToDouble(i.AMO3);
            }
            recmon.SAL_AMT = SAL_AMT.ToString(); recmon.TAX_AMT = TAX_AMT.ToString(); recmon.AMO1 = AMO1.ToString(); recmon.AMO2 = AMO2.ToString(); recmon.AMO3 = AMO3.ToString();
            if (recmon.REC_AMT == null || recmon.REC_AMT == "")
                recmon.REC_AMT = "0";
            recmon.TOT_AMT = (Convert.ToDouble(recmon.REC_AMT) - SAL_AMT - TAX_AMT).ToString();
            db.SaveChanges();
        }
        public ITRANS SalesConvertITRANS(Sales sales)
        {
            var itrans = new ITRANS();
            itrans.ID = (int)sales.ITEM_ID;
            itrans.TRN_NO = sales.TRN_NO;
            itrans.ITEM_NO = sales.ITEM_NO;
            itrans.QTY = sales.QTY;
            itrans.PRICE = sales.PRICE;
            itrans.AMOUNT = sales.AMOUNT;
            itrans.CODE = sales.CODE;
            itrans.ACC_DATE = sales.ACC_DATE;
            itrans.ACC_YN = sales.ACC_YN;
            itrans.TRN_DATE = sales.TRN_DATE;
            itrans.ORD_NO = sales.ORD_NO;
            itrans.SAL_NO = sales.SAL_NO;
            itrans.SAL_CODE = sales.SAL_CODE;
            return itrans;
        }
        public INVOICE SalesConvertInvoice(Sales sales)
        {
            var inv = new INVOICE();
            inv.TRN_NO = sales.TRN_NO;
            inv.INV_NO = sales.INV_NO;
            inv.TRN_DATE = sales.TRN_DATE;
            inv.CODE = sales.CODE;
            inv.TAX = sales.TAX;
            inv.REMARK1 = sales.REMARK1;
            inv.ACC_DATE = sales.ACC_DATE;
            inv.PAY_WAY = sales.PAY_WAY;
            inv.SUMAMT = sales.SUMAMT;
            inv.TAXAMT = sales.TAXAMT;         
            inv.TOTAMT = sales.TOTAMT;
            inv.SAL_NO = sales.SAL_NO;
            inv.AMO1 = sales.AMO1;
            inv.AMO2 = sales.AMO2;
            inv.AMO3 = sales.AMO3;
            return inv;
        }
    }
}