using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class CUST_ItemWithName
    {
        public int id { get; set; }
        public string CUST_CODE { get; set; }
        public string CUST_NAME { get; set; }
        public string ITEM_NO { get; set; }
        public string ITEM_NAME { get; set; }
        public string SAL_CODE { get; set; }
        public string L_DATE { get; set; }
        public Nullable<double> L_PRICE { get; set; }
        public Nullable<double> L_QTY { get; set; }
        public Nullable<double> COST { get; set; }
        public string REMARK { get; set; }
    }
}