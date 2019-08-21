using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class RECAMTController : ApiController
    {        
        private invEntities db = new invEntities();
       
        public RECAMT Get(string TRN_DATE)
        {
            // 拆解日期
            var code_ary = TRN_DATE.Split('.');
            var search_string = code_ary[0] + code_ary[1];
            var p = db.RECAMT.Where(m => m.REC_NO.StartsWith(search_string)).OrderByDescending(m => m.REC_NO).FirstOrDefault();
            return p;
        }
        // GET: api/RECAMT/5
        public RECAMT2 Get(string REC_NO, int preOrNext)
        {
            if (preOrNext == 0)
            {
                var search = (from d in db.RECAMT join c in db.CUSTOMER on d.CUST_CODE   equals c.CUST_CODE
                              where string.Compare(d.REC_NO, REC_NO) < 0
                              select new RECAMT2 {
                                  id=d.id,
                                  REC_NO=d.REC_NO,
                                  CUST_CODE=d.CUST_CODE,
                                  CUST_NAME=c.CUST_NAME,
                                  REC_MON=d.REC_MON,
                                  TRN_DATE=d.TRN_DATE,
                                  REC_AMT=d.REC_AMT,
                                  CL_AMT=d.CL_AMT,
                                  CHNO=d.CHNO,
                                  CASH=d.CASH,
                                  DISC=d.DISC,
                                  OTH_REC=d.OTH_REC,
                                  D_NO=d.D_NO,
                                  RMARK=d.RMARK,
                                  REMARK1=d.REMARK1
                              }).OrderByDescending(d => d.REC_NO).FirstOrDefault();
                return search;
            }
            else
            {
                var search = (from d in db.RECAMT
                              join c in db.CUSTOMER on d.CUST_CODE equals c.CUST_CODE
                              where string.Compare(d.REC_NO, REC_NO) > 0
                              select new RECAMT2
                              {
                                  id = d.id,
                                  REC_NO = d.REC_NO,
                                  CUST_CODE = d.CUST_CODE,
                                  CUST_NAME = c.CUST_NAME,
                                  REC_MON = d.REC_MON,
                                  TRN_DATE = d.TRN_DATE,
                                  REC_AMT = d.REC_AMT,
                                  CL_AMT = d.CL_AMT,
                                  CHNO = d.CHNO,
                                  CASH = d.CASH,
                                  DISC = d.DISC,
                                  OTH_REC = d.OTH_REC,
                                  D_NO = d.D_NO,
                                  RMARK = d.RMARK,
                                  REMARK1 = d.REMARK1
                              }).OrderBy(d => d.REC_NO).FirstOrDefault();
                return search;
            }
        }

        // POST: api/RECAMT
        public List<RECAMT2> Post([FromBody]RECAMT search)
        {
            var s = search.REC_NO;
            var p = (from d in db.RECAMT
                     join c in db.CUSTOMER on d.CUST_CODE equals c.CUST_CODE
                     select new RECAMT2
                     {
                         id = d.id,
                         REC_NO = d.REC_NO,
                         CUST_CODE = d.CUST_CODE,
                         CUST_NAME = c.CUST_NAME,
                         REC_MON = d.REC_MON,
                         TRN_DATE = d.TRN_DATE,
                         REC_AMT = d.REC_AMT,
                         CL_AMT = d.CL_AMT,
                         CHNO = d.CHNO,
                         CASH = d.CASH,
                         DISC = d.DISC,
                         OTH_REC = d.OTH_REC,
                         D_NO = d.D_NO,
                         RMARK = d.RMARK,
                         REMARK1 = d.REMARK1
                     }).OrderByDescending(d => d.REC_NO);
            if (search!=null &&s!=null &&s!="")
            {
                p = (from d in db.RECAMT
                         join c in db.CUSTOMER on d.CUST_CODE equals c.CUST_CODE
                         where
                         d.REC_NO.StartsWith(s) ||
                         d.REC_MON.StartsWith(s) ||
                         d.CUST_CODE.StartsWith(s) ||
                         c.CUST_NAME.StartsWith(s) ||
                         d.TRN_DATE.StartsWith(s)
                         select new RECAMT2
                         {
                             id = d.id,
                             REC_NO = d.REC_NO,
                             CUST_CODE = d.CUST_CODE,
                             CUST_NAME = c.CUST_NAME,
                             REC_MON = d.REC_MON,
                             TRN_DATE = d.TRN_DATE,
                             REC_AMT = d.REC_AMT,
                             CL_AMT = d.CL_AMT,
                             CHNO = d.CHNO,
                             CASH = d.CASH,
                             DISC = d.DISC,
                             OTH_REC = d.OTH_REC,
                             D_NO = d.D_NO,
                             RMARK = d.RMARK,
                             REMARK1 = d.REMARK1
                         }).OrderByDescending(d => d.REC_NO);
            }
            // 查詢
            
            return p.Take(200).ToList();
        }

        // PUT: api/RECAMT/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/RECAMT/5
        public void Delete(int id)
        {
        }
    }
}
