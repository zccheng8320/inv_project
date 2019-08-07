using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INV_Project.Models;
namespace INV_Project.Controllers
{
    public class FactoryController : Controller
    {
        private invEntities db = new invEntities();
        // GET: Factory
        public ActionResult Index(int FID=0)
        {
            try
            {
                FID = int.Parse(Request.Cookies["FID"].Value.ToString());
            }
            catch (Exception e)
            {
                FID = 0;
            }
            var p = (from d in db.FACTORY where d.ID == FID select d).FirstOrDefault();
            if (FID != 0)
            {
                HttpCookie Cookie = new HttpCookie("FID", p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            else
            {
                p = (from d in db.FACTORY select d).OrderByDescending(d => d.ID).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie("FID", p.ID.ToString());
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
        public ActionResult Insert(string FACT_CODE, string FACT_NAME, string ADDRESS1, string ADDRESS2, string UNIFORM, string PAY_WAY, string ATTENTION
            , string TELEPHONE, string FAX, string REMARK, string TRN_DATE)
        {
            if (FACT_CODE != "")
            {
                var search = (from d in db.FACTORY select d).OrderByDescending(d => d.ID).FirstOrDefault();
                int id = search.ID + 1;
                FACTORY p = new FACTORY();
                p.ID = id;
                p.FACT_CODE = FACT_CODE;
                p.FACT_NAME = FACT_NAME;
                p.ADDRESS1 = ADDRESS1;
                p.ADDRESS2 = ADDRESS2;
                p.UNIFORM = UNIFORM;
                p.PAY_WAY = PAY_WAY;
                p.ATTENTION = ATTENTION;
                p.TELEPHONE = TELEPHONE;
                p.FAX = FAX;
                p.REMARK = REMARK;
                p.TRN_DATE = TRN_DATE;
                db.FACTORY.Add(p);
                db.SaveChanges();
                HttpCookie Cookie = new HttpCookie("FID", p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var p = (from d in db.FACTORY where d.ID == id select d).FirstOrDefault();
            return View(p);

        }
        [HttpPost]
        public ActionResult Update(int id, string FACT_CODE, string FACT_NAME, string ADDRESS1, string ADDRESS2, string UNIFORM, string PAY_WAY, string ATTENTION
            , string TELEPHONE,  string FAX,  string REMARK)
        {
            var p = db.FACTORY.Where(m => m.ID == id).FirstOrDefault();
            p.FACT_CODE = FACT_CODE;
            p.FACT_NAME = FACT_NAME;
            p.ADDRESS1 = ADDRESS1;
            p.ADDRESS2 = ADDRESS2;
            p.UNIFORM = UNIFORM;
            p.PAY_WAY = PAY_WAY;
            p.ATTENTION = ATTENTION;
            p.TELEPHONE = TELEPHONE;
            p.FAX = FAX;
            p.REMARK = REMARK;
            HttpCookie Cookie = new HttpCookie("FID", p.ID.ToString());
            Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
            HttpContext.Response.Cookies.Add(Cookie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id, int password)
        {
            if (password == 618320)
            {
                var p = (from d in db.FACTORY where d.ID == id select d).FirstOrDefault();
                db.FACTORY.Remove(p);
                db.SaveChanges();
                var search = (from d in db.FACTORY where d.ID < id select d).OrderByDescending(d => d.ID).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie("FID", search.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult FactItem(string FACT_CODE)
        {
            var FACT_NAME = (from ci in db.FACTORY where ci.FACT_CODE == FACT_CODE select ci.FACT_NAME).FirstOrDefault();
            TempData["FACT_CODE"] = FACT_CODE;
            TempData["FACT_NAME"] = FACT_NAME;
            var p = from ci in db.FACTITEM
                    join i in db.ITEM on ci.ITEM_NO equals i.ITEM_NO
                    where ci.FACT_CODE == FACT_CODE
                    orderby ci.L_DATE descending
                    select new FACT_ItemWithName
                    {
                        id = ci.id,
                        FACT_CODE = ci.FACT_CODE,
                        ITEM_NO = ci.ITEM_NO,
                        ITEM_NAME = i.NAME1,
                        L_DATE = ci.L_DATE,
                        L_PRICE = ci.L_PRICE,
                        L_QTY = ci.L_QTY,
                        COST = ci.COST,
                        REMARK = ci.REMARK
                    };
            return View(p.ToList());
        }
        public ActionResult FactItemDel(int id)
        {
            var p = db.FACTITEM.Where(m => m.id == id).FirstOrDefault();
            string FACT_CODE = p.FACT_CODE;
            db.FACTITEM.Remove(p);
            db.SaveChanges();
            return RedirectToAction("FactItem", new { FACT_CODE = FACT_CODE });
        }
        public ActionResult FactREC(string CODE)
        {
            var np = db.FACTORY.Where(m => m.FACT_CODE == CODE).FirstOrDefault();
            var FACT_NAME = np.FACT_NAME;
            TempData["FACT_CODE"] = CODE;
            TempData["FACT_NAME"] = FACT_NAME;
            var t1 = from c in db.PAYMON
                     where c.FACT_CODE == CODE
                     select c;
            var t2 = from c in db.PAYAMT where c.FACT_CODE == CODE select c;
            var p = from c1 in t1
                    join c2 in t2 on c1.MON_DATE equals c2.REC_MON into g
                    from c2 in g.DefaultIfEmpty()
                    orderby c1.MON_DATE descending
                    select new FactREC
                    {
                        FACT_CODE = c1.FACT_CODE,
                        MON_DATE = c1.MON_DATE,
                        SAL_AMT = c1.SAL_AMT,
                        TAX_AMT = c1.TAX_AMT,
                        REC_AMT = c1.REC_AMT,
                        TOT_AMT = c1.TOT_AMT,
                        AMO1 = c1.AMO1,
                        AMO2 = c1.AMO2,
                        AMO3 = c1.AMO3,
                        DISC = c1.DISC,
                        REC_NO = c2.REC_NO,
                        TRN_DATE = c2.TRN_DATE,
                        CHNO = c2.CHNO,
                        CASH = c2.CASH,
                    };
            return View(p.ToList());
        }
    }
}