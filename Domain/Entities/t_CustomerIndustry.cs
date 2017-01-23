namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_CustomerIndustry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_CustomerIndustry()
        {
            t_Customers = new HashSet<t_Customers>();
        }

        [Key]
        public Guid s_CustomerIndustryID { get; set; }

        public string s_CustomerIndustryName { get; set; }

        public string s_CustomerIndustryRemark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Customers> t_Customers { get; set; }
    }
}
