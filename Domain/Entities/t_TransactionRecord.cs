namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_TransactionRecord
    {
        [Key]
        [StringLength(50)]
        public string s_TransactionID { get; set; }

        [StringLength(30)]
        public string s_CustomerID { get; set; }

        [StringLength(30)]
        public string s_OrderID { get; set; }

        public byte s_SettlementMode { get; set; }

        public DateTime s_TransTime { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Debit { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Credit { get; set; }

        [Column(TypeName = "money")]
        public decimal s_CurrentBalance { get; set; }

        [Column(TypeName = "money")]
        public decimal s_TransAmount { get; set; }

        [StringLength(100)]
        public string s_TransDetail { get; set; }

        [StringLength(30)]
        public string t_MonthlyStatement_s_StatementID { get; set; }

        public byte s_PayType { get; set; }

        public byte s_TransType { get; set; }

        public virtual t_Customers t_Customers { get; set; }

        public virtual t_MonthlyStatement t_MonthlyStatement { get; set; }

        public virtual t_PayType t_PayType { get; set; }
    }
}
