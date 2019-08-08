using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INV_Project.Models;
namespace INV_Project.Controllers
{
    public class ItemController : Controller
    {
        private invEntities db = new invEntities();
        private string CookieID = "IID";
        // GET: Item
        public ActionResult Index(int IID =0)
        {
            try
            {
                IID = int.Parse(Request.Cookies[CookieID].Value.ToString());
            }
            catch (Exception e)
            {
                IID = 0;
            }
            var p = (from d in db.ITEM where d.ID == IID select d).FirstOrDefault();
            if (IID != 0)
            {
                HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                p = (from d in db.ITEM select d).OrderByDescending(d => d.ITEM_NO).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            return View(p);
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(string ITEM_NO, string CLASS, string NAME1, string NAME2, double S_PRICE, double C_PRICE, double U_PRICE, string UNIT, double QTY, double S1_QTY, double S2_QTY
            , double BLA_QTY, double P_QTY, double R_QTY, double G_QTY, double I_QTY, double SQTY)
        {
            var check = db.ITEM.Where(m => m.ITEM_NO == ITEM_NO).FirstOrDefault();
            if(check==null && ITEM_NO != "")
            {
                var search = (from d in db.ITEM select d).OrderByDescending(d => d.ID).FirstOrDefault();
                int id = search.ID + 1;
                ITEM p = new ITEM();
                p.ID = id;
                p.ITEM_NO = ITEM_NO;
                p.CLASS = CLASS;
                p.NAME1 = NAME1;
                p.NAME2 = NAME2;
                p.S_PRICE = S_PRICE;
                p.C_PRICE = C_PRICE;
                p.U_PRICE = U_PRICE;
                p.UNIT = UNIT;
                p.QTY = QTY;
                p.S1_QTY = S1_QTY;
                p.S2_QTY = S2_QTY;
                p.BLA_QTY = BLA_QTY;
                p.P_QTY = P_QTY;
                p.R_QTY = R_QTY;
                p.I_QTY = I_QTY;
                p.SQTY = SQTY;
                p.TRN_DATE = "";
                p.SDATE = "";
                db.ITEM.Add(p);
                db.SaveChanges();
                HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                TempData["Inser_Code"] = 1;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var p = (from d in db.ITEM where d.ID == id select d).FirstOrDefault();
            return View(p);

        }
        [HttpPost]
        public ActionResult Update(int id, string CLASS, string NAME1, string NAME2, double S_PRICE, double C_PRICE, double U_PRICE, string UNIT, double QTY, double S1_QTY, double S2_QTY
            , double BLA_QTY, double P_QTY, double R_QTY, double G_QTY, double I_QTY, double SQTY)
        {
            var p = db.ITEM.Where(m => m.ID == id).FirstOrDefault();
            p.CLASS = CLASS;
            p.NAME1 = NAME1;
            p.NAME2 = NAME2;
            p.S_PRICE = S_PRICE;
            p.C_PRICE = C_PRICE;
            p.U_PRICE = U_PRICE;
            p.UNIT = UNIT;
            p.QTY = QTY;
            p.S1_QTY = S1_QTY;
            p.S2_QTY = S2_QTY;
            p.BLA_QTY = BLA_QTY;
            p.P_QTY = P_QTY;
            p.R_QTY = R_QTY;
            p.I_QTY = I_QTY;
            p.SQTY = SQTY;
            p.TRN_DATE = "";
            p.SDATE = "";
            db.SaveChanges();
            HttpCookie Cookie = new HttpCookie(CookieID, p.ID.ToString());
            Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
            HttpContext.Response.Cookies.Add(Cookie);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id, int password)
        {
            if (password == 618320)
            {
                var p = (from d in db.ITEM where d.ID == id select d).FirstOrDefault();
                db.ITEM.Remove(p);
                db.SaveChanges();
                var search = (from d in db.ITEM where d.ID < id select d).OrderByDescending(d => d.ID).FirstOrDefault();
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
        public ActionResult Ptrans(string ITEM_NO)
        {
            var t1 = db.PTRANS.Where(m => m.ITEM_NO == ITEM_NO);
            var t2 = from p in db.PTRANS where p.ITEM_NO == ITEM_NO group p by p.CODE into g select new { CODE=g.Key, TRN_DATE = g.Max(p => p.TRN_DATE) };
            var a = from p1 in t1 join p2 in t2 on new { p1.CODE, p1.TRN_DATE } equals new { p2.CODE, p2.TRN_DATE } select p1;
            var f = from f1 in db.FACTORY join f2 in a on f1.FACT_CODE equals f2.CODE orderby f2.TRN_DATE descending
                    select new PTRANSwithName {TRN_NO=f2.TRN_NO, CODE = f2.CODE, NAME = f1.FACT_NAME, TRN_DATE = f2.TRN_DATE, PRICE = f2.PRICE, QTY = f2.QTY };
            return View(f.ToList());
        }
        public ActionResult Itrans(string ITEM_NO)
        {
            var t1 = db.ITRANS.Where(m => m.ITEM_NO == ITEM_NO);
            var t2 = from p in db.ITRANS where p.ITEM_NO == ITEM_NO group p by p.CODE into g select new { CODE = g.Key, TRN_DATE = g.Max(p => p.TRN_DATE) };
            var a = from p1 in t1 join p2 in t2 on new { p1.CODE, p1.TRN_DATE } equals new { p2.CODE, p2.TRN_DATE } select p1;
            var f = from f1 in db.CUSTOMER join f2 in a on f1.CUST_CODE equals f2.CODE orderby f2.TRN_DATE descending
                    select new ITRANSWithName { TRN_NO = f2.TRN_NO, CODE = f2.CODE, NAME = f1.CUST_NAME, TRN_DATE = f2.TRN_DATE, PRICE = f2.PRICE, QTY = f2.QTY,COST=f2.COST,SAL_CODE=f2.SAL_CODE,ORD_NO=f2.ORD_NO };
            return View(f.ToList());
        }
    }
}
