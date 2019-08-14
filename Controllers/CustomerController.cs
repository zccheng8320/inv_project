using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INV_Project.Models;
namespace INV_Project.Controllers
{
    public class CustomerController : Controller
    {
        private invEntities db = new invEntities();
        private string CookieID = "ID";
        // GET: Customer
        public ActionResult Index(int ID=0)
        {
            try {
                ID = int.Parse(Request.Cookies[CookieID].Value.ToString());
            }
            catch(Exception e)
            {
                ID = 0;
            }
            var p = (from d in db.CUSTOMER where d.ID == ID select d).FirstOrDefault();
            if (ID != 0)
            {
                HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }              
            else
            {
                p = (from d in db.CUSTOMER select d).OrderByDescending(d => d.ID).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }      
            return View(p);
        }
        public ActionResult Search(int id=0)
        {
            var search2 = (from d in db.CUSTOMER where d.ID >= id select d).OrderBy(d => d.ID).Take(100);
            return View(search2.ToList());
        }
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(string CUST_CODE, string CUST_NAME, string ADDRESS1, string ADDRESS2, string UNIFORM, string PAY_WAY, string ATTENTION
            , string TELEPHONE, string SAL_NO, string FAX, string PAY_DATE, string PRT_LAB, string REMARK)
        {
            var check = db.CUSTOMER.Where(m => m.CUST_CODE == CUST_CODE).FirstOrDefault();
            if ( CUST_CODE != "")
            {
                if (check == null)
                {
                    var search = (from d in db.CUSTOMER select d).OrderByDescending(d => d.ID).FirstOrDefault();
                    int id = search.ID + 1;
                    CUSTOMER p = new CUSTOMER();
                    p.ID = id;
                    p.CUST_CODE = CUST_CODE;
                    p.CUST_NAME = CUST_NAME;
                    p.ADDRESS1 = ADDRESS1;
                    p.ADDRESS2 = ADDRESS2;
                    p.UNIFORM = UNIFORM;
                    p.PAY_WAY = PAY_WAY;
                    p.ATTENTION = ATTENTION;
                    p.TELEPHONE = TELEPHONE;
                    p.SAL_NO = SAL_NO;
                    p.FAX = FAX;
                    p.PAY_DATE = PAY_DATE;
                    p.PRT_LAB = PRT_LAB;
                    p.REMARK = REMARK;
                    p.TRN_DATE = "";
                    db.CUSTOMER.Add(p);
                    db.SaveChanges();
                    HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
                    Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                    HttpContext.Response.Cookies.Add(Cookie);
                    // 0 代表失敗
                }
                else
                    TempData["Inser_Code"] = 1;
            }
            
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var p = (from d in db.CUSTOMER where d.ID == id select d).FirstOrDefault();
            return View(p);

        }
        [HttpPost]
        public ActionResult Update(int id,string CUST_NAME,string ADDRESS1,string ADDRESS2,string UNIFORM,string PAY_WAY,string ATTENTION
            ,string TELEPHONE,string SAL_NO,string FAX,string PAY_DATE,string PRT_LAB,string REMARK)
        {
            
            var p = db.CUSTOMER.Where(m => m.ID == id).FirstOrDefault();
            p.CUST_NAME = CUST_NAME;
            p.ADDRESS1 = ADDRESS1;
            p.ADDRESS2 = ADDRESS2;
            p.UNIFORM = UNIFORM;
            p.PAY_WAY = PAY_WAY;
            p.ATTENTION = ATTENTION;
            p.TELEPHONE = TELEPHONE;
            p.SAL_NO = SAL_NO;
            p.FAX = FAX;
            p.PAY_DATE = PAY_DATE;
            p.PRT_LAB = PRT_LAB;
            p.REMARK = REMARK;
            HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
            Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
            HttpContext.Response.Cookies.Add(Cookie);
            db.SaveChanges();
                     
            return RedirectToAction("Index");

        }
        public ActionResult Delete(int id,int password)
        {
            if(password == 618320)
            {
                var p = (from d in db.CUSTOMER where d.ID == id select d).FirstOrDefault();
                db.CUSTOMER.Remove(p);
                db.SaveChanges();
                var search = (from d in db.CUSTOMER where d.ID < id select d).OrderByDescending(d => d.ID).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie(CookieID, search.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }                     
        }
        public ActionResult CustItem(string CUST_CODE)
        {
            var CUST_NAME = (from ci in db.CUSTOMER where ci.CUST_CODE == CUST_CODE select ci.CUST_NAME).FirstOrDefault();
            TempData["CUST_CODE"] = CUST_CODE;
            TempData["CUST_NAME"] = CUST_NAME;
            var p = from ci in db.CUSTITEM
                    join i in db.ITEM on ci.ITEM_NO equals i.ITEM_NO
                    join c in db.CUSTOMER on ci.CUST_CODE equals c.CUST_CODE where ci.CUST_CODE == CUST_CODE orderby ci.L_DATE descending
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
        public ActionResult CustREC(string CODE)
        {
            var np=db.CUSTOMER.Where(m => m.CUST_CODE == CODE).FirstOrDefault();
            var CUST_NAME = np.CUST_NAME;
            TempData["CUST_CODE"] = CODE;
            TempData["CUST_NAME"] = CUST_NAME;
            var t1 = from c in db.RECMON
                    where c.CUST_CODE == CODE
                    select c;
            var t2 = from c in db.RECAMT where c.CUST_CODE == CODE select c;
            var p = from c1 in t1
                    join c2 in t2 on c1.MON_DATE equals c2.REC_MON into g
                    from c2 in g.DefaultIfEmpty() orderby c1.MON_DATE descending
                    select new CustREC
                    {
                        CUST_CODE = c1.CUST_CODE,
                        MON_DATE = c1.MON_DATE,
                        SAL_AMT = c1.SAL_AMT,
                        TAX_AMT = c1.TAX_AMT,
                        REC_AMT = c1.REC_AMT,
                        TOT_AMT = c1.TOT_AMT,
                        BAL1 = c1.BAL1,
                        AMO1 = c1.AMO1,
                        AMO2 = c1.AMO2,
                        AMO3 = c1.AMO3,
                        DISC = c1.DISC,
                        NANO = c1.NANO,
                        REC_NO = c2.REC_NO,
                        TRN_DATE = c2.TRN_DATE,
                        CHNO = c2.CHNO,
                        CASH = c2.CASH,
                        D_NO = c2.D_NO,
                    };
            return View(p.ToList());
        }

    }
}