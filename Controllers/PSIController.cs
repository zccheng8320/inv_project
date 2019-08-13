using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INV_Project.Models;
namespace INV_Project.Controllers
{
    public class PSIController : Controller
    {
        private invEntities db = new invEntities();
        // GET: PSI
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sales()
        {
            var TRN_NO = db.INVOICE.FirstOrDefault().TRN_NO;
            //var TRN_NO = "10806001";
            var table = (from inv in db.INVOICE
                     join itran in db.ITRANS on inv.TRN_NO equals itran.TRN_NO into table1
                     from t1 in table1.DefaultIfEmpty()
                         join c in db.CUSTOMER on t1.CODE equals c.CUST_CODE into table2
                     from t2 in table2.DefaultIfEmpty()
                         join e in db.EMPLOY on t1.SAL_NO equals e.SAL_NO into table3
                     from t3 in table3.DefaultIfEmpty()
                         join i in db.ITEM on t1.ITEM_NO equals i.ITEM_NO into table4
                     from t4 in table4 .DefaultIfEmpty() where inv.TRN_NO ==TRN_NO
                     select new Sales
                     {
                         TRN_NO = inv.TRN_NO,
                         INV_NO = inv.INV_NO,
                         TRN_DATE=inv.TRN_DATE,
                         CODE = inv.CODE,
                         CUST_NAME=t2.CUST_NAME,
                         TELEPHONE=t2.TELEPHONE,
                         UNIFORM=t2.UNIFORM,
                         ATTENTION=t2.ATTENTION,
                         TAX = inv.TAX,
                         REMARK1 = inv.REMARK1,
                         ACC_DATE = inv.ACC_DATE,
                         PAY_WAY = inv.PAY_WAY,
                         SUMAMT = inv.SUMAMT,
                         TAXAMT = inv.TAXAMT,
                         TOTAMT = inv.TOTAMT,
                         SAL_NO = inv.SAL_NO,
                         SAL_NAME=t3.NAME,
                         ITEM_NO = t1.ITEM_NO,
                         ITEM_NAME=t4.NAME1,
                         QTY = t1.QTY,
                         PRICE = t1.PRICE,
                         AMOUNT=t1.AMOUNT,
                         ACC_YN = inv.ACC_YN,
                         ORD_NO = t1.ORD_NO

                     }).ToList();         
            return View(table);
        }
    }
}