﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using INV_Project.Models;
namespace INV_Project.Controllers.API
{
    public class EmployController : ApiController
    {
        private invEntities db = new invEntities();
        // GET: api/Employ
        public List<EMPLOY> Get(string search)
        {
            // 查詢
            var p = db.EMPLOY.Where(m =>
            m.SAL_NO.Contains(search)
            || m.NAME.Contains(search)
            || m.ADDRESS1.Contains(search)
            || m.SEX.Contains(search)
            || m.TEL1.Contains(search)
            || m.BBCALL.Contains(search)).Take(200);
            return p.ToList();
        }

        // GET: api/Employ/5
        public EMPLOY Get(string SAL_NO, int preOrNext)
        {
            if (preOrNext == 0)
            {
                var search = (from d in db.EMPLOY where string.Compare(d.SAL_NO , SAL_NO)<0 select d).OrderByDescending(d => d.SAL_NO).FirstOrDefault();
                return search;
            }
            else
            {
                var search = (from d in db.EMPLOY where string.Compare(d.SAL_NO, SAL_NO) > 0 select d).FirstOrDefault();
                return search;
            }
        }



        // POST: api/Employ
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Employ/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Employ/5
        public void Delete(int id)
        {
        }
    }
}
