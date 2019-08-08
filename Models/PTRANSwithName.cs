using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class PTRANSwithName
    {
        public int ID { get; set; }
        public string TRN_NO { get; set; }
        public string ITEM_NO { get; set; }
        public string QTY { get; set; }
        public string PRICE { get; set; }
        public string AMOUNT { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string ACC_DATE { get; set; }
        public string ACC_YN { get; set; }
        public string TRN_DATE { get; set; }
        public string ORD_NO { get; set; }
        public string PRD_NO { get; set; }
        public string SHIP_DATE { get; set; }
        public string ST_NO { get; set; }
        public string COST { get; set; }
    }
}