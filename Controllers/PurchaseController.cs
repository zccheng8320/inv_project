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
    public class PurchaseController : Controller
    {
        private invEntities db = new invEntities();
        private string CookieID = "PTRN_NO";
        // GET: Purchase
        public ActionResult Index(string TRN_NO = "")
        {
            var fTRN_NO = db.PURCH.FirstOrDefault().TRN_NO;
            try
            {
                if(TRN_NO=="")
                    TRN_NO = Request.Cookies[CookieID].Value.ToString();
            }
            catch (Exception e)
            {
                TRN_NO = "";
            }
            var table = (from purch in db.PURCH
                         join ptrans in db.PTRANS on purch.TRN_NO equals ptrans.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.FACTORY on purch.CODE equals c.FACT_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where purch.TRN_NO == TRN_NO
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
            Debug.WriteLine(TRN_NO);
            if (TRN_NO != "")
            {
                HttpCookie Cookie = new HttpCookie(CookieID, table.FirstOrDefault().TRN_NO);
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                table = (from purch in db.PURCH
                         join ptrans in db.PTRANS on purch.TRN_NO equals ptrans.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.FACTORY on purch.CODE equals c.FACT_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where purch.TRN_NO == fTRN_NO
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
                HttpCookie Cookie = new HttpCookie(CookieID, table.FirstOrDefault().TRN_NO);
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            return View(table);
        }
        public ActionResult Insert()
        {
            return View();
        }
        public ActionResult Update(string TRN_NO = "")
        {
            var table = (from purch in db.PURCH
                         join ptrans in db.PTRANS on purch.TRN_NO equals ptrans.TRN_NO into table1
                         from t1 in table1.DefaultIfEmpty()
                         join c in db.FACTORY on purch.CODE equals c.FACT_CODE into table2
                         from t2 in table2.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                         from t4 in table4.DefaultIfEmpty()
                         where purch.TRN_NO == TRN_NO
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
            return View(table);
        }
        [HttpPost]
        public void Update_Purchase([FromBody]List<PURCHASE> salesList)
        {
            var SF = salesList.FirstOrDefault();
            UpdatePURCH(SF);
            db.SaveChanges();
            foreach (var s in salesList)
            {
                if (s.ITEM_ID != null)
                    UpdatePTRANS(s);
                var factitem = db.FACTITEM.Where(m => m.FACT_CODE == s.CODE && m.ITEM_NO == s.ITEM_NO).FirstOrDefault();
                if (factitem != null)
                {
                    factitem.L_PRICE = Convert.ToDouble(s.PRICE);
                    factitem.L_DATE = s.TRN_DATE;
                    factitem.L_QTY = Convert.ToDouble(s.QTY);
                    factitem.REMARK = s.ORD_NO;
                }
                db.SaveChanges();
            }
            UpdatePAYMON(SF);
        }
        public void UpdatePTRANS(PURCHASE sales)
        {
            var ptrans = db.PTRANS.Where(m => m.ID == sales.ITEM_ID).FirstOrDefault();
            ptrans.ID = (int)sales.ITEM_ID;
            ptrans.TRN_NO = sales.TRN_NO;
            ptrans.ITEM_NO = sales.ITEM_NO;
            ptrans.QTY = sales.QTY;
            ptrans.PRICE = sales.PRICE;
            ptrans.AMOUNT = sales.AMOUNT;
            ptrans.CODE = sales.CODE;
            ptrans.ACC_DATE = sales.ACC_DATE;
            ptrans.ACC_YN = sales.ACC_YN;
            ptrans.TRN_DATE = sales.TRN_DATE;
            ptrans.ORD_NO = sales.ORD_NO;
            db.SaveChanges();
        }
        public void UpdatePURCH(PURCHASE sales)
        {
            var purch = db.PURCH.Where(m => m.TRN_NO == sales.TRN_NO).FirstOrDefault();
            purch.TRN_NO = sales.TRN_NO;
            purch.INV_NO = sales.INV_NO;
            purch.TRN_DATE = sales.TRN_DATE;
            purch.CODE = sales.CODE;
            purch.TAX = sales.TAX;
            purch.REMARK1 = sales.REMARK1;
            purch.ACC_DATE = sales.ACC_DATE;
            purch.PAY_WAY = sales.PAY_WAY;
            purch.SUMAMT = sales.SUMAMT;
            purch.TAXAMT = sales.TAXAMT;
            purch.TOTAMT = sales.TOTAMT;
            purch.AMO1 = sales.AMO1;
            purch.AMO2 = sales.AMO2;
            purch.AMO3 = sales.AMO3;
            db.SaveChanges();
        }
        public void UpdatePAYMON(PURCHASE s)
        {
            var paymon = db.PAYMON.Where(m => m.FACT_CODE == s.CODE && m.MON_DATE == s.ACC_DATE).FirstOrDefault();
            var purch_list = db.PURCH.Where(m => m.CODE == s.CODE && m.ACC_DATE == s.ACC_DATE).ToList();
            var SAL_AMT = 0.0; var TAX_AMT = 0.0; var AMO1 = 0.0; var AMO2 = 0.0; var AMO3 = 0.0;
            foreach (var i in purch_list)
            {
                SAL_AMT += Convert.ToDouble(i.SUMAMT);
                TAX_AMT += Convert.ToDouble(i.TAXAMT);
                AMO1 += Convert.ToDouble(i.AMO1);
                AMO2 += Convert.ToDouble(i.AMO2);
                AMO3 += Convert.ToDouble(i.AMO3);
            }
            paymon.SAL_AMT = SAL_AMT.ToString();
            paymon.TAX_AMT = TAX_AMT.ToString();
            paymon.AMO1 = AMO1.ToString();
            paymon.AMO2 = AMO2.ToString();
            paymon.AMO3 = AMO3.ToString();
            if (paymon.REC_AMT == null || paymon.REC_AMT == "")
                paymon.REC_AMT = "0";
            paymon.TOT_AMT = (SAL_AMT + TAX_AMT - Convert.ToDouble(paymon.REC_AMT)).ToString();
            db.SaveChanges();
        }
    }
}