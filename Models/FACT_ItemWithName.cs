﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class FACT_ItemWithName
    {
        public int id { get; set; }
        public string FACT_CODE { get; set; }
        public string ITEM_NO { get; set; }
        public string ITEM_NAME { get; set; }
        public string L_DATE { get; set; }
        public string L_PRICE { get; set; }
        public string L_QTY { get; set; }
        public string COST { get; set; }
        public string REMARK { get; set; }
    }
}