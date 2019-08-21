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
            var CUST_NAME = db.CUSTOMER.Where(m => m.CUST_CODE == p.CUST_CODE).DefaultIfEmpty().FirstOrDefault().CUST_NAME;
            if (RECAMT_ID != 0)
            {
                TempData["CUST_NAME"] = CUST_NAME;
                HttpCookie Cookie = new HttpCookie(CookieID, p.id.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                p = (from d in db.RECAMT select d).OrderByDescending(d => d.id).FirstOrDefault();
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
    }
}