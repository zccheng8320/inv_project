﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class invEntities : DbContext
    {
        public invEntities()
            : base("name=invEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CLASS> CLASS { get; set; }
        public virtual DbSet<EMPLOY> EMPLOY { get; set; }
        public virtual DbSet<FACTITEM> FACTITEM { get; set; }
        public virtual DbSet<FACTORY> FACTORY { get; set; }
        public virtual DbSet<INVOICE> INVOICE { get; set; }
        public virtual DbSet<ITEM> ITEM { get; set; }
        public virtual DbSet<MONTOT> MONTOT { get; set; }
        public virtual DbSet<NCUST> NCUST { get; set; }
        public virtual DbSet<PAYAMT> PAYAMT { get; set; }
        public virtual DbSet<PAYMON> PAYMON { get; set; }
        public virtual DbSet<PHRASE> PHRASE { get; set; }
        public virtual DbSet<PTRANS> PTRANS { get; set; }
        public virtual DbSet<PURCH> PURCH { get; set; }
        public virtual DbSet<RECAMT> RECAMT { get; set; }
        public virtual DbSet<RECMON> RECMON { get; set; }
        public virtual DbSet<CUSTOMER> CUSTOMER { get; set; }
        public virtual DbSet<CUSTITEM> CUSTITEM { get; set; }
        public virtual DbSet<ITRANS> ITRANS { get; set; }

        public System.Data.Entity.DbSet<INV_Project.Models.CUST_ItemWithName> CUST_ItemWithName { get; set; }

        public System.Data.Entity.DbSet<INV_Project.Models.CUST_AR> CUST_AR { get; set; }
    }
}
