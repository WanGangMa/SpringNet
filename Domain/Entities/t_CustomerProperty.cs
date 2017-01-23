namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_CustomerProperty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_CustomerProperty()
        {
            t_Customers = new HashSet<t_Customers>();
        }

        [Key]
        public Guid s_CustomerPropertyID { get; set; }

        public string s_CustomerPropertyName { get; set; }

        public string s_CustomerPropertyRemark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Customers> t_Customers { get; set; }
    }
}
