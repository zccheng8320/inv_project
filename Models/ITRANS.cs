//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace INV_Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ITRANS
    {
        public int ID { get; set; }
        public string TRN_NO { get; set; }
        public string ITEM_NO { get; set; }
        public string QTY { get; set; }
        public Nullable<double> PRICE { get; set; }
        public Nullable<double> AMOUNT { get; set; }
        public string CODE { get; set; }
        public string ACC_DATE { get; set; }
        public string ACC_YN { get; set; }
        public string TRN_DATE { get; set; }
        public string ORD_NO { get; set; }
        public string PRD_NO { get; set; }
        public string SHIP_DATE { get; set; }
        public string ST_NO { get; set; }
        public string SAL_NO { get; set; }
        public string SAL_CODE { get; set; }
        public string COST { get; set; }
    }
}
