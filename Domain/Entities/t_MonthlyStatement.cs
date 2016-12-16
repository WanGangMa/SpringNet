namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_MonthlyStatement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_MonthlyStatement()
        {
            t_TransactionRecord = new HashSet<t_TransactionRecord>();
        }

        [Key]
        [StringLength(30)]
        public string s_StatementID { get; set; }

        [StringLength(20)]
        public string s_StatementName { get; set; }

        [StringLength(30)]
        public string s_CustomerID { get; set; }

        public DateTime s_StartDate { get; set; }

        public DateTime s_EndDate { get; set; }

        [Column(TypeName = "money")]
        public decimal s_LastBalance { get; set; }

        [Column(TypeName = "money")]
        public decimal s_CurrentPayment { get; set; }

        [Column(TypeName = "money")]
        public decimal s_CurrentPaid { get; set; }

        [Column(TypeName = "money")]
        public decimal s_CurrentBalance { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Arrears { get; set; }

        public decimal s_SurplusRefund { get; set; }

        public byte s_StatementState { get; set; }

        public DateTime s_CreateTime { get; set; }

        public virtual t_Customers t_Customers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_TransactionRecord> t_TransactionRecord { get; set; }
    }
}
