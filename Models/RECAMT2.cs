using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class RECAMT2
    {
        public int id { get; set; }
        public string REC_NO { get; set; }
        public string CUST_CODE { get; set; }
        public string CUST_NAME { get; set; }
        public string REC_MON { get; set; }
        public string TRN_DATE { get; set; }
        public string REC_AMT { get; set; }
        public Nullable<double> CL_AMT { get; set; }
        public string CHNO { get; set; }
        public string CASH { get; set; }
        public Nullable<double> DISC { get; set; }
        public string OTH_REC { get; set; }
        public string D_NO { get; set; }
        public string RMARK { get; set; }
        public string REMARK1 { get; set; }
    }
}