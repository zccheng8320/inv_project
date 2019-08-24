using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
using INV_Project.Controllers;
using System.Diagnostics;

namespace INV_Project.Controllers.API
{
    public class PurchaseInsertController : ApiController
    {
        private invEntities db = new invEntities();
        private PurchaseController pc = new PurchaseController();
        // GET: api/PurchaseInsert
        public PURCH Get(string TRN_DATE)
        {
            // 拆解日期
            var code_ary = TRN_DATE.Split('.');
            var search_string = code_ary[0] + code_ary[1];
            var p = db.PURCH.Where(m => m.TRN_NO.StartsWith(search_string)).OrderByDescending(m => m.ID).FirstOrDefault();
            return p;
        }

        // GET: api/PurchaseInsert/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PurchaseInsert
        [HttpPost]
        public void Post([FromBody]List<PURCHASE> purchaseList)
        {
            // 取得民國日期           
            var today = getToday();
            var ym = getToday("ym");
            // 第一筆資料
            var sf = purchaseList.FirstOrDefault();
            var purch = db.PURCH;
            var ptrans = db.PTRANS;
            // 應收款([paymon]) 新增或更新
            var paymon = db.PAYMON.Where(m => m.FACT_CODE == sf.CODE && m.MON_DATE == sf.ACC_DATE).FirstOrDefault();
            if (paymon != null)
            {
                paymon.SAL_AMT = (Convert.ToDouble(paymon.SAL_AMT) + sf.SUMAMT).ToString();
                paymon.TAX_AMT = (Convert.ToDouble(paymon.TAX_AMT) + sf.TAXAMT).ToString();
                paymon.TOT_AMT = (Convert.ToDouble(paymon.TOT_AMT) + sf.TOTAMT).ToString();
                paymon.AMO1 = (Convert.ToDouble(paymon.AMO1) + sf.AMO1).ToString();
                paymon.AMO2 = (Convert.ToDouble(paymon.AMO2) + sf.AMO2).ToString();
                paymon.AMO3 = (Convert.ToDouble(paymon.AMO3) + sf.AMO3).ToString();
            }
            else
                db.PAYMON.Add(new PAYMON { FACT_CODE = sf.CODE, MON_DATE = sf.ACC_DATE, SAL_AMT = sf.SUMAMT.ToString(), TAX_AMT = sf.TAXAMT.ToString(), TOT_AMT = sf.TOTAMT.ToString(), AMO1 = sf.AMO1.ToString(), AMO2 = sf.AMO2.ToString(), AMO3 = sf.AMO3.ToString() });
            // Factory TRN_DATE Update
            var factory = db.FACTORY.Where(m => m.FACT_CODE == sf.CODE).FirstOrDefault();
            factory.TRN_DATE = today;
            // PURCH Insert
            purch.Add(new PURCH { TRN_NO = sf.TRN_NO, INV_NO = sf.INV_NO, TRN_DATE = sf.TRN_DATE, CODE = sf.CODE, TAX = sf.TAX, REMARK1 = sf.REMARK1, ACC_DATE = sf.ACC_DATE, SUMAMT = sf.SUMAMT, TAXAMT = sf.TAXAMT, TOTAMT = sf.TOTAMT, AMO1 = sf.AMO1, AMO2 = sf.AMO2, AMO3 = sf.AMO3, PAY_WAY = sf.PAY_WAY, ACC_YN = sf.ACC_YN });
            // 
            foreach (var s in purchaseList)
            {
                if(s.QTY!=null &s.QTY!="")
                    ItemAddAction(s, today);
            }
            db.SaveChanges();

        }
        public string getToday(string i = "today")
        {
            // 取得民國日期
            var year = DateTime.Now.Year - 1911;
            var month = "" + DateTime.Now.Month;
            var day = "" + DateTime.Now.Day;
            if (DateTime.Now.Month < 10)
            { month = "0" + month; }
            if (DateTime.Now.Day < 10)
            { day = "0" + day; }

            var today = year + "." + month + "." + day;
            var ym = year + "." + month;
            if (i == "ym")
                return ym;
            else
                return today;
        }
       
        // PUT: api/PurchaseInsert/5
        public string Put([FromBody]PURCHASE s)
        {
            string ID = "";
            var today = getToday();
            if (s.ITEM_ID == 0)
            {
                ID = ItemAddAction(s, today);

            }
            else
            {
                var factitem = db.FACTITEM.Where(m => m.FACT_CODE == s.CODE && m.ITEM_NO == s.ITEM_NO).FirstOrDefault();
                if (factitem != null)
                {
                    factitem.L_PRICE = Convert.ToDouble(s.PRICE);
                    factitem.L_DATE = s.TRN_DATE;
                    factitem.L_QTY = Convert.ToDouble(s.QTY);
                    factitem.REMARK = s.ORD_NO;
                }
                var i = db.PTRANS.Where(m => m.ID == s.ITEM_ID).FirstOrDefault();
                i.PRICE = s.PRICE;
                i.QTY = s.QTY;
                i.AMOUNT = s.AMOUNT;
                i.ORD_NO = s.ORD_NO;
                ID = s.ITEM_ID.ToString();
            }
            var purch = db.PURCH.Where(m => m.TRN_NO == s.TRN_NO).FirstOrDefault();
            purch.SUMAMT = s.SUMAMT; purch.AMO1 = s.AMO1;
            purch.TAXAMT = s.TAXAMT; purch.AMO2 = s.AMO2;
            purch.TOTAMT = s.TOTAMT; purch.AMO3 = s.AMO3;
            db.SaveChanges();
            pc.UpdatePAYMON(s);
            return ID;
        }
        public string ItemAddAction(PURCHASE p, string today)
        {
            var item = db.ITEM.Where(m => m.ITEM_NO == p.ITEM_NO).FirstOrDefault();
            // 減去庫存量
            var n_QTY = item.QTY + Convert.ToDouble(p.QTY);
            item.QTY = n_QTY;
            item.TRN_DATE = today;
            // 更新歷史成交價 或新增
            var factitem = db.FACTITEM.Where(m => m.FACT_CODE == p.CODE && m.ITEM_NO == p.ITEM_NO).FirstOrDefault();
            if (factitem != null)
            {
                factitem.L_PRICE = Convert.ToDouble(p.PRICE);
                factitem.L_DATE = p.TRN_DATE;
                factitem.L_QTY = Convert.ToDouble(p.QTY);
                factitem.COST = 0;
                factitem.REMARK = p.ORD_NO;

            }
            else
                db.FACTITEM.Add(new FACTITEM { FACT_CODE = p.CODE, ITEM_NO = p.ITEM_NO, L_QTY = Convert.ToDouble(p.QTY), L_DATE = p.TRN_DATE, L_PRICE = Convert.ToDouble(p.PRICE), COST = 0, REMARK = p.ORD_NO });
            var i = new PTRANS { TRN_NO = p.TRN_NO, ITEM_NO = p.ITEM_NO, QTY = p.QTY, PRICE = p.PRICE, AMOUNT = p.AMOUNT, CODE = p.CODE, ACC_DATE = p.ACC_DATE, ACC_YN = p.ACC_YN, TRN_DATE = p.TRN_DATE, ORD_NO = p.ORD_NO, COST = "0" };
            db.PTRANS.Add(i);
            db.SaveChanges();
            return i.ID.ToString();
        }
        // DELETE: api/PurchaseInsert/5
        public void Delete([FromBody]PURCHASE s)
        {
            Debug.WriteLine(s.ITEM_ID);
            var ptrans = db.PTRANS.Where(m => m.ID == s.ITEM_ID).FirstOrDefault();
            db.PTRANS.Remove(ptrans);
            var purch = db.PURCH.Where(m => m.TRN_NO == s.TRN_NO).FirstOrDefault();
            purch.SUMAMT = s.SUMAMT; purch.AMO1 = s.AMO1;
            purch.TAXAMT = s.TAXAMT; purch.AMO2 = s.AMO2;
            purch.TOTAMT = s.TOTAMT; purch.AMO3 = s.AMO3;
            db.SaveChanges();
            // 更新PAYMON
            pc.UpdatePAYMON(s);
        }
    }
}
