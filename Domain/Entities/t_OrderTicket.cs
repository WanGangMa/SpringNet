namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_OrderTicket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_OrderTicket()
        {
            t_Orders = new HashSet<t_Orders>();
        }

        [Key]
        public Guid s_TicketID { get; set; }

        [StringLength(30)]
        public string s_OrderID { get; set; }

        public byte s_PayType { get; set; }

        public string s_PartyA { get; set; }

        public string s_PartyB { get; set; }

        public string s_SerialNumber { get; set; }

        public decimal s_TotalMoney { get; set; }

        public string s_Remark { get; set; }

        public DateTime? s_AddTime { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Orders> t_Orders { get; set; }

        public virtual t_PayType t_PayType { get; set; }

        public virtual t_Users t_Users { get; set; }
    }
}
