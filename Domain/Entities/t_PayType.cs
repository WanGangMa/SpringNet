namespace Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_PayType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_PayType()
        {
            t_Orders = new HashSet<t_Orders>();
            t_OrderTicket = new HashSet<t_OrderTicket>();
            t_TransactionRecord = new HashSet<t_TransactionRecord>();
        }

        [Key]
        public byte s_PayType { get; set; }

        [StringLength(15)]
        public string s_TypeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Orders> t_Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_OrderTicket> t_OrderTicket { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_TransactionRecord> t_TransactionRecord { get; set; }
    }
}
