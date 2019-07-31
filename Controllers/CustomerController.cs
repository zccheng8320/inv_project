using System;
using System.Collections.Generic;
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
        public ActionResult Index(string CUST_CODE=null)
        {
            var p = (from d in db.CUSTOMER select d).OrderByDescending(d => d.CUST_CODE).FirstOrDefault();
            if (CUST_CODE !=null)
            {
                p = (from d in db.CUSTOMER where d.CUST_CODE == CUST_CODE select d).FirstOrDefault();
                TempData["CUST_CODE"] = p.CUST_CODE;
            }              
            else
            {
                var a = TempData["CUST_CODE"];
                if (a != null)
                    p = (from d in db.CUSTOMER where d.CUST_CODE == a.ToString() select d).FirstOrDefault();
                else
                    TempData["CUST_CODE"] = p.CUST_CODE;
            }      
            return View(p);
        }
        public ActionResult Search()
        {
            return View();
        }
        
    }
}