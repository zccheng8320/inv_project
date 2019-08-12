using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INV_Project.Models;
namespace INV_Project.Controllers
{
    public class EmployController : Controller
    {
        private invEntities db = new invEntities();
        private string CookieID = "SAL_NO";
        // GET: Employ
        public ActionResult Index(string SAL_NO = "")
        {
            try
            {
                SAL_NO = Request.Cookies[CookieID].Value.ToString();
            }
            catch (Exception e)
            {
                SAL_NO = "";
            }
            var p = (from d in db.EMPLOY where d.SAL_NO == SAL_NO select d).FirstOrDefault();
            if (SAL_NO != "")
            {
                HttpCookie Cookie = new HttpCookie(CookieID, p.SAL_NO.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                p = (from d in db.EMPLOY select d).OrderByDescending(d => d.SAL_NO).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie(CookieID, p.SAL_NO.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            return View(p);
        }
        public ActionResult Search()
        {
            var p = (from d in db.EMPLOY select d).Take(40).ToList();
            return View(p);
        }
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(string SAL_NO, string NAME, string SEX, string BLOOD, string B_PLACE, string BIRTHDAY, string ID_NO
           , string MARRY, string ADDRESS1, string TEL1, string BBCALL, string ADDRESS2, string TEL2, string EDUCAT
            , string EXPERIENC, string UNIT, string JOB, string A_DATE, string L_DATE, string ATTENTION, string RELATION, string REL_ADDR
            , string REL_TEL)
        {
            var check = db.EMPLOY.Where(m => m.SAL_NO == SAL_NO).FirstOrDefault();
            if(SAL_NO != "")
            {
                if (check == null)
                {
                    EMPLOY p = new EMPLOY();
                    p.SAL_NO = SAL_NO;
                    p.NAME = NAME;
                    p.SEX = SEX;
                    p.BLOOD = BLOOD;
                    p.B_PLACE = B_PLACE;
                    p.BIRTHDAY = BIRTHDAY;
                    p.ID_NO = ID_NO;
                    p.MARRY = MARRY;
                    p.ADDRESS1 = ADDRESS1;
                    p.TEL1 = TEL1;
                    p.BBCALL = BBCALL;
                    p.ADDRESS2 = ADDRESS2;
                    p.EDUCAT = EDUCAT;
                    p.UNIT = UNIT;
                    p.JOB = JOB;
                    p.A_DATE = A_DATE;
                    p.L_DATE = L_DATE;
                    p.EXPERIENC = EXPERIENC;
                    p.TEL2 = TEL2;
                    p.ATTENTION = ATTENTION;
                    p.RELATION = RELATION;
                    p.REL_ADDR = REL_ADDR;
                    p.REL_TEL = REL_TEL;
                    db.EMPLOY.Add(p);
                    db.SaveChanges();
                    HttpCookie Cookie = new HttpCookie(CookieID, p.SAL_NO.ToString());
                    Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                    HttpContext.Response.Cookies.Add(Cookie);
                    // 0 代表失敗
                }
                else
                    TempData["Inser_Code"] = 1;
            }
            
            return RedirectToAction("Index");
        }
        public ActionResult Update(string SAL_NO)
        {
            var p = (from d in db.EMPLOY where d.SAL_NO == SAL_NO select d).FirstOrDefault();
            return View(p);

        }
        [HttpPost]
        public ActionResult Update(string SAL_NO, string NAME, string SEX, string BLOOD, string B_PLACE, string BIRTHDAY, string ID_NO
           , string MARRY, string ADDRESS1, string TEL1, string BBCALL, string ADDRESS2, string TEL2, string EDUCAT
            , string EXPERIENC, string UNIT, string JOB, string A_DATE, string L_DATE, string ATTENTION, string RELATION, string REL_ADDR
            , string REL_TEL)
        {
            var p = db.EMPLOY.Where(m => m.SAL_NO == SAL_NO).FirstOrDefault();
            p.NAME = NAME;
            p.SEX = SEX;
            p.BLOOD = BLOOD;
            p.B_PLACE = B_PLACE;
            p.BIRTHDAY = BIRTHDAY;
            p.ID_NO = ID_NO;
            p.MARRY = MARRY;
            p.ADDRESS1 = ADDRESS1;
            p.TEL1 = TEL1;
            p.BBCALL = BBCALL;
            p.ADDRESS2 = ADDRESS2;
            p.EDUCAT = EDUCAT;
            p.UNIT = UNIT;
            p.JOB = JOB;
            p.A_DATE = A_DATE;
            p.L_DATE = L_DATE;
            p.EXPERIENC = EXPERIENC;
            p.TEL2 = TEL2;
            p.ATTENTION = ATTENTION;
            p.RELATION = RELATION;
            p.REL_ADDR = REL_ADDR;
            p.REL_TEL = REL_TEL;
            db.SaveChanges();
            HttpCookie Cookie = new HttpCookie(CookieID, p.SAL_NO.ToString());
            Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
            HttpContext.Response.Cookies.Add(Cookie);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string SAL_NO, int password)
        {
            if (password == 618320)
            {
                var p = (from d in db.EMPLOY where d.SAL_NO == SAL_NO select d).FirstOrDefault();
                db.EMPLOY.Remove(p);
                db.SaveChanges();
                var search = (from d in db.EMPLOY where string.Compare(d.SAL_NO, SAL_NO) < 0 select d).OrderByDescending(d => d.SAL_NO).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie(CookieID, search.SAL_NO.ToString());
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