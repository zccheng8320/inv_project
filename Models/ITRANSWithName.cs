using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class ITRANSWithName
    {
        public int ID { get; set; }
        public string TRN_NO { get; set; }
        public string ITEM_NO { get; set; }
        public string QTY { get; set; }
        public Nullable<double> PRICE { get; set; }
        public Nullable<double> AMOUNT { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string ACC_DATE { get; set; }
        public string ACC_YN { get; set; }
        public string TRN_DATE { get; set; }
        public string ORD_NO { get; set; }
        public string PRD_NO { get; set; }
        public string SHIP_DATE { get; set; }
        public string ST_NO { get; set; }
        public string SAL_NO { get; set; }
        public string SAL_CODE { get; set; }
        public Nullable<double> COST { get; set; }
    }
}