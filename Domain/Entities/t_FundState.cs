namespace Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_FundState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_FundState()
        {
            t_Orders = new HashSet<t_Orders>();
        }

        [Key]
        public byte s_FundState { get; set; }

        [StringLength(15)]
        public string s_FundStateName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Orders> t_Orders { get; set; }
    }
}
