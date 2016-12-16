namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Bills
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Bills()
        {
            t_Orders = new HashSet<t_Orders>();
        }

        [Key]
        [StringLength(50)]
        public string s_BillID { get; set; }

        [StringLength(30)]
        public string s_CustomerID { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Amount { get; set; }

        [StringLength(500)]
        public string s_BillDetail { get; set; }

        [StringLength(20)]
        public string s_AddUser { get; set; }

        public DateTime s_AddTime { get; set; }

        [StringLength(20)]
        public string s_EditUser { get; set; }

        public DateTime? s_EditTime { get; set; }

        public byte s_BillState { get; set; }

        public string s_Remark { get; set; }

        [StringLength(500)]
        public string s_BillItems { get; set; }

        public virtual t_BillState t_BillState { get; set; }

        public virtual t_Customers t_Customers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Orders> t_Orders { get; set; }
    }
}
