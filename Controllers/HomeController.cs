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
    }
}