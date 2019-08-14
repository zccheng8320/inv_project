using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class Sales
    {       
        public string TRN_NO { get; set; }
        public string INV_NO { get; set; }
        public string TRN_DATE { get; set; }
        public string CODE { get; set; }
        public string CUST_NAME { get; set; }
        public string ATTENTION { get; set; }
        public string TELEPHONE { get; set; }
        public string UNIFORM { get; set; }
        public Nullable<double> TAX { get; set; }
        public string REMARK1 { get; set; }
        public string ACC_DATE { get; set; }
        public string ACC_YN { get; set; }
        public string PAY_WAY { get; set; }
        public Nullable<double> SUMAMT { get; set; }
        public Nullable<double> TAXAMT { get; set; }
        public Nullable<double> TOTAMT { get; set; }
        public string SAL_NO { get; set; }
        public string SAL_NAME { get; set; }
        public string ITEM_NO { get; set; }
        public string ITEM_NAME { get; set; }
        public string QTY { get; set; }
        public Nullable<double> PRICE { get; set; }
        public Nullable<double> AMOUNT { get; set; }
        public string ORD_NO { get; set; }
        public string SAL_CODE { get; set; }
        public Nullable<double> COST { get; set; }
    }
}