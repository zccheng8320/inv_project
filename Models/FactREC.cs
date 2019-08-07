using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class FactREC
    {
        public int ID { get; set; }
        public string FACT_CODE { get; set; }
        public string MON_DATE { get; set; }
        public string SAL_AMT { get; set; }
        public string TAX_AMT { get; set; }
        public string REC_AMT { get; set; }
        public string TOT_AMT { get; set; }
        public string AMO1 { get; set; }
        public string AMO2 { get; set; }
        public string AMO3 { get; set; }
        public string DISC { get; set; }
        public string REC_NO { get; set; }
        public string TRN_DATE { get; set; }
        public string CHNO { get; set; }
        public string CASH { get; set; }
    }
}