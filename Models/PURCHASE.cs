using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class PURCHASE
    {
        public string TRN_NO { get; set; }
        public string INV_NO { get; set; }
        public string TRN_DATE { get; set; }
        public string CODE { get; set; }
        public string FACT_NAME { get; set; }
        public string ATTENTION { get; set; }
        public string TELEPHONE { get; set; }
        public string UNIFORM { get; set; }
        public string TAX { get; set; }
        public string REMARK1 { get; set; }
        public string ACC_DATE { get; set; }
        public string ACC_YN { get; set; }
        public string PAY_WAY { get; set; }
        public string SUMAMT { get; set; }
        public string TAXAMT { get; set; }
        public string TOTAMT { get; set; }
        public string SAL_NO { get; set; }
        public string SAL_NAME { get; set; }
        public string ITEM_NO { get; set; }
        public string ITEM_NAME { get; set; }
        public string QTY { get; set; }
        public string PRICE { get; set; }
        public string AMOUNT { get; set; }
        public string ORD_NO { get; set; }
        public string SAL_CODE { get; set; }
        public string COST { get; set; }
        public string AMO1 { get; set; }
        public string AMO2 { get; set; }
        public string AMO3 { get; set; }
        public Nullable<int> ITEM_ID { get; set; }
    }
}