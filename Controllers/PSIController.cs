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
                             COST = t4.C_PRICE,
                             ITEM_ID=t1.ID,
                             AMO1=inv.AMO1,
                             AMO2=inv.AMO2,
                             AMO3=inv.AMO3
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
                             ORD_NO = t1.ORD_NO,
                             ITEM_ID = t1.ID,
                             AMO1 = inv.AMO1,
                             AMO2 = inv.AMO2,
                             AMO3 = inv.AMO3

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
        public ActionResult CustItem(string CUST_CODE)
        {
            var CUST_NAME = (from ci in db.CUSTOMER where ci.CUST_CODE == CUST_CODE select ci.CUST_NAME).FirstOrDefault();
            TempData["CUST_CODE"] = CUST_CODE;
            TempData["CUST_NAME"] = CUST_NAME;
            var p = from ci in db.CUSTITEM
                    join i in db.ITEM on ci.ITEM_NO equals i.ITEM_NO
                    join c in db.CUSTOMER on ci.CUST_CODE equals c.CUST_CODE
                    where ci.CUST_CODE == CUST_CODE
                    orderby ci.L_DATE descending
                    select new CUST_ItemWithName
                    {
                        id = ci.id,
                        CUST_CODE = ci.CUST_CODE,
                        CUST_NAME = c.CUST_NAME,
                        ITEM_NO = ci.ITEM_NO,
                        ITEM_NAME = i.NAME1,
                        SAL_CODE = ci.SAL_CODE,
                        L_DATE = ci.L_DATE,
                        L_PRICE = ci.L_PRICE,
                        L_QTY = ci.L_QTY,
                        COST = ci.COST,
                        REMARK = ci.REMARK
                    };
            return View(p.ToList());
        }
        public ActionResult CustItemDel(int id)
        {
            var p = db.CUSTITEM.Where(m => m.id == id).FirstOrDefault();
            string CUST_CODE = p.CUST_CODE;
            db.CUSTITEM.Remove(p);
            db.SaveChanges();
            return RedirectToAction("CustItem", new { CUST_CODE = CUST_CODE });
        }
        [HttpPost]
        public void Update_Sales([FromBody]List<Sales> salesList)
        {            
            var SF = salesList.FirstOrDefault();           
            UpdateInvoice(SF);
            db.SaveChanges();
            foreach(var s in salesList)
            {
                if (s.ITEM_ID != null)
                    UpdateITRANS(s);
                var custitem = db.CUSTITEM.Where(m => m.CUST_CODE == s.CODE && m.ITEM_NO == s.ITEM_NO).FirstOrDefault();
                if (custitem != null)
                {
                    custitem.L_PRICE = s.PRICE;
                    custitem.L_DATE = s.TRN_DATE;
                    custitem.L_QTY = Convert.ToDouble(s.QTY);
                    custitem.SAL_CODE = s.SAL_CODE;
                    custitem.REMARK = s.ORD_NO;
                }
                db.SaveChanges();
            }              
            UpdateRECMON(SF);
        }
        [HttpPost]
        public string Delete([FromBody]List<Sales> salesList,int password)
        {
            if(password==618320)
            {
                var SF = salesList.FirstOrDefault();
                var invoice = db.INVOICE.Where(m => m.TRN_NO == SF.TRN_NO).FirstOrDefault();
                db.INVOICE.Remove(invoice);
                foreach (var i in salesList)
                {
                    if (i.QTY!=null && i.QTY!="")
                    {
                        var itrans = db.ITRANS.Where(m => m.ID == i.ITEM_ID).FirstOrDefault();
                        db.ITRANS.Remove(itrans);
                        db.SaveChanges();
                    }                    
                }
                UpdateRECMON(SF);
                var TRN_NO = (from d in db.INVOICE where string.Compare(d.TRN_NO, SF.TRN_NO) < 0 select d).OrderByDescending(d => d.ID).FirstOrDefault().TRN_NO;
                HttpCookie Cookie = new HttpCookie(CookieID, TRN_NO);
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
                return "刪除成功";
            }        
            else
            {
                return "密碼錯誤";
            }
        }
        public void UpdateITRANS(Sales sales)
        {
            var itrans = db.ITRANS.Where(m => m.ID == sales.ITEM_ID).FirstOrDefault();
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
            db.SaveChanges();
        }
        public void UpdateInvoice(Sales sales)
        {
            var inv = db.INVOICE.Where(m => m.TRN_NO == sales.TRN_NO).FirstOrDefault();
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
            db.SaveChanges();
        }
        public void UpdateRECMON(Sales s)
        {
            var recmon = db.RECMON.Where(m => m.CUST_CODE == s.CODE && m.MON_DATE == s.ACC_DATE).FirstOrDefault();
            var inv_list = db.INVOICE.Where(m => m.CODE == s.CODE && m.ACC_DATE == s.ACC_DATE).ToList();
            var SAL_AMT = 0.0; var TAX_AMT = 0.0; var AMO1 = 0.0; var AMO2 = 0.0; var AMO3 = 0.0;
            foreach (var i in inv_list)
            {
                SAL_AMT += Convert.ToDouble(i.SUMAMT);
                TAX_AMT += Convert.ToDouble(i.TAXAMT);
                AMO1 += Convert.ToDouble(i.AMO1);
                AMO2 += Convert.ToDouble(i.AMO2);
                AMO3 += Convert.ToDouble(i.AMO3);
            }
            recmon.SAL_AMT = SAL_AMT.ToString();
            recmon.TAX_AMT = TAX_AMT.ToString();
            recmon.AMO1 = AMO1.ToString();
            recmon.AMO2 = AMO2.ToString();
            recmon.AMO3 = AMO3.ToString();
            if (recmon.REC_AMT == null || recmon.REC_AMT == "")
                recmon.REC_AMT = "0";
            recmon.TOT_AMT = (SAL_AMT + TAX_AMT- Convert.ToDouble(recmon.REC_AMT)).ToString();
            db.SaveChanges();
        }
    }
}