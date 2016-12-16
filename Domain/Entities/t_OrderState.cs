namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_OrderState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_OrderState()
        {
            t_Orders = new HashSet<t_Orders>();
        }

        [Key]
        public byte s_OrderState { get; set; }

        [StringLength(15)]
        public string s_StateName { get; set; }

        public byte s_AccessBill { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Orders> t_Orders { get; set; }
    }
}
