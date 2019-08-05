using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INV_Project.Models
{
    public class CUST_AR
    {
        public int id { get; set; }
        public string month { get; set; }
        public Nullable<double> CL_Amount { get; set; }
        public Nullable<double> TAX { get; set; }
        public Nullable<double>  REC_Amount { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> Machine { get; set; }
        public Nullable<double> Material { get; set; }
        public Nullable<double> Service { get; set; }
    }
}