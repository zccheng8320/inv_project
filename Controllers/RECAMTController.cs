using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using INV_Project.Models;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace INV_Project.Controllers
{
    public class RECAMTController : Controller
    {
        private invEntities db = new invEntities();
        private string CookieID = "RECAMT_ID";
        // GET: RECAMT
        public ActionResult Index(int RECAMT_ID=0)
        {
           
            try
            {
                if(RECAMT_ID==0)
                    RECAMT_ID = int.Parse(Request.Cookies[CookieID].Value.ToString());
            }
            catch (Exception e)
            {
                RECAMT_ID = 0;
            }
            var p = (from d in db.RECAMT where d.id == RECAMT_ID select d).FirstOrDefault();
            Debug.WriteLine("cust_code=" + RECAMT_ID);
            
            if (RECAMT_ID != 0)
            {
                var CUST_NAME = db.CUSTOMER.Where(m => m.CUST_CODE == p.CUST_CODE).DefaultIfEmpty().FirstOrDefault().CUST_NAME;
                TempData["CUST_NAME"] = CUST_NAME;
                HttpCookie Cookie = new HttpCookie(CookieID, p.id.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                p = (from d in db.RECAMT select d).OrderByDescending(d => d.id).FirstOrDefault();
                var CUST_NAME = db.CUSTOMER.Where(m => m.CUST_CODE == p.CUST_CODE).DefaultIfEmpty().FirstOrDefault().CUST_NAME;
                CUST_NAME = db.CUSTOMER.Where(m => m.CUST_CODE == p.CUST_CODE).FirstOrDefault().CUST_NAME;
                TempData["CUST_NAME"] = CUST_NAME;
                HttpCookie Cookie = new HttpCookie(CookieID, p.id.ToString());
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
        public string Insert_RECAMT([FromBody] RECAMT r)
        {
            db.RECAMT.Add(r);
            var recmon = db.RECMON.Where(m => m.CUST_CODE == r.CUST_CODE && m.MON_DATE == r.REC_MON).FirstOrDefault();
            recmon.REC_AMT = r.CL_AMT.ToString();
            recmon.DISC = r.DISC.ToString();
            var SAL_AMT = Convert.ToDouble(recmon.SAL_AMT);
            var TAX_AMT= Convert.ToDouble(recmon.TAX_AMT);
            var DISC = (double)r.DISC;
            recmon.TOT_AMT = (SAL_AMT + TAX_AMT - DISC - (double)r.CL_AMT).ToString();
            db.SaveChanges();
            return r.id.ToString();
        }
        public ActionResult Update(int RECAMT_ID=1)
        {
            var p = db.RECAMT.Where(m => m.id == RECAMT_ID).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public ActionResult Update(RECAMT r)
        {
            var p = db.RECAMT.Where(m => m.id == r.id).FirstOrDefault();
            p.CASH = r.CASH;    p.CHNO = r.CHNO;    p.D_NO = r.D_NO;    p.REMARK1 = r.REMARK1;
            db.SaveChanges();
            var CUST_NAME = db.CUSTOMER.Where(m => m.CUST_CODE == p.CUST_CODE).FirstOrDefault().CUST_NAME;
            TempData["CUST_NAME"] = CUST_NAME;
            HttpCookie Cookie = new HttpCookie(CookieID, p.id.ToString());
            Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
            HttpContext.Response.Cookies.Add(Cookie);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int RECAMT_ID, int password)
        {
            if (password == 618320)
            {
                var p = (from d in db.RECAMT where d.id == RECAMT_ID select d).FirstOrDefault();
                db.RECAMT.Remove(p);
                db.SaveChanges();
                var recmon=db.RECMON.Where(m => m.CUST_CODE == p.CUST_CODE && m.MON_DATE == p.REC_MON).FirstOrDefault();
                recmon.REC_AMT = "0";
                recmon.DISC = "0";
                var SAL_AMT = Convert.ToDouble(recmon.SAL_AMT);
                var TAX_AMT = Convert.ToDouble(recmon.TAX_AMT);
                recmon.TOT_AMT = (SAL_AMT + TAX_AMT).ToString();
                db.SaveChanges();
                var search = (from d in db.RECAMT where string.Compare(d.REC_NO, p.REC_NO) < 0 select d).OrderByDescending(d => d.REC_NO).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie(CookieID, search.id.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}