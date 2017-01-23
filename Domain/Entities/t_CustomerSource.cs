namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_CustomerSource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_CustomerSource()
        {
            t_Customers = new HashSet<t_Customers>();
        }

        [Key]
        public Guid s_CustomerSourceID { get; set; }

        public string s_CustomerSourceName { get; set; }

        public string s_CustomerSourceRemark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Customers> t_Customers { get; set; }
    }
}
