using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INV_Project.Models;
namespace INV_Project.Controllers
{
    public class HomeController : Controller
    {
        private invEntities db = new invEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Customer()
        {
            var a = TempData["CUST_CODE"];
            var p = (from d in db.CUSTOMER select d).OrderByDescending(d => d.CUST_CODE).FirstOrDefault();
            if (a != null)
                p = (from d in db.CUSTOMER where d.CUST_CODE == a select d).FirstOrDefault();
            else
                TempData["CUST_CODE"] = p.CUST_CODE;
            Debug.WriteLine(p.CUST_CODE);
            return View(p);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}