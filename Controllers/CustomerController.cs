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
        // GET: Customer
        public ActionResult Index(int ID=1)
        {
            try {
                ID = int.Parse(Request.Cookies["ID"].Value.ToString());
            }
            catch(Exception e)
            {
                ID = 1;
            }
            var p = (from d in db.CUSTOMER where d.ID == ID select d).FirstOrDefault();
            if (ID != 1)
            {
                HttpCookie Cookie = new HttpCookie("ID", p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }              
            else
            {
                p = (from d in db.CUSTOMER select d).OrderByDescending(d => d.ID).FirstOrDefault();
                HttpCookie Cookie = new HttpCookie("ID", p.ID.ToString());
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
        public ActionResult Insert(string CUST_CODE, string CUST_NAME, string ADDRESS1, string ADDRESS2, string UNIFORM, string PAY_WAY, string ATTENTION
            , string TELEPHONE, string SAL_NO, string FAX, string PAY_DATE, string PRT_LAB, string REMARK, string TRN_DATE)
        {
            if (CUST_CODE != "")
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
                p.TRN_DATE = TRN_DATE;
                db.CUSTOMER.Add(p);
                db.SaveChanges();
                HttpCookie Cookie = new HttpCookie("ID", p.ID.ToString());
                Cookie.Expires = DateTime.Now.AddDays(1); //設置Cookie到期時間
                HttpContext.Response.Cookies.Add(Cookie);
            }         
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id)
        {
            var p = (from d in db.CUSTOMER where d.ID == id select d).FirstOrDefault();
            return View(p);

        }
        [HttpPost]
        public ActionResult Update(int id,string CUST_CODE,string CUST_NAME,string ADDRESS1,string ADDRESS2,string UNIFORM,string PAY_WAY,string ATTENTION
            ,string TELEPHONE,string SAL_NO,string FAX,string PAY_DATE,string PRT_LAB,string REMARK,string TRN_DATE)
        {
            var p = db.CUSTOMER.Where(m => m.ID == id).FirstOrDefault();
            p.CUST_CODE=CUST_CODE;
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
            p.TRN_DATE = TRN_DATE;
            HttpCookie Cookie = new HttpCookie("ID", p.ID.ToString());
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
                HttpCookie Cookie = new HttpCookie("ID", search.ID.ToString());
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
        public ActionResult CustAR(string CODE)
        {
            double TAX = Convert.ToDouble(db.CUSTOMER.Where(m => m.CUST_CODE == CODE).FirstOrDefault().TAX);
            var ACC_DATE = db.ITRANS.Where(m => m.CODE == CODE).Select(m=>m.ACC_DATE).Distinct().ToList();
            List<CUST_AR> CA_List = new List<CUST_AR>();
            for(var i=0;i<ACC_DATE.Count();i++)
            {
                CUST_AR ar = new CUST_AR();
                ar.id = i;
                ar.month = ACC_DATE[i];
                ar.Machine = 0;
                ar.Material = 0;
                ar.Service = 0;
                ar.Discount = 0;
                ar.CL_Amount = 0; ;
                var p = db.ITRANS.Where(m => m.CODE == CODE && m.ACC_DATE == ar.month).ToList();
                foreach(var a in p)
                {
                    if (a.ITEM_NO[0] == '1')
                        ar.Machine += (double)a.AMOUNT;
                    else if (a.ITEM_NO[0] == '2')
                        ar.Material += (double)a.AMOUNT;
                    else if (a.ITEM_NO[0] == '3')
                        ar.Service += (double)a.AMOUNT;
                }
                ar.REC_Amount = ar.Machine + ar.Material + ar.Service;
                ar.TAX = Math.Ceiling((double)ar.REC_Amount * (TAX / 100));
                var r= db.RECAMT.Where(m => m.CUST_CODE == CODE && m.REC_MON == ar.month).FirstOrDefault();
                if(r!=null)
                {
                    ar.CL_Amount = double.Parse(r.CL_AMT);
                    ar.Discount = double.Parse(r.DISC);
                }
                Debug.WriteLine("銷貨金額"+ar.REC_Amount);
                Debug.WriteLine("稅金" + ar.TAX);
                Debug.WriteLine("沖帳金額" + ar.CL_Amount);
                Debug.WriteLine("折抵" + ar.Discount);
                Debug.WriteLine("----------------------------");
                CA_List.Append(ar);
            }
            return View(CA_List);
        }

    }
}