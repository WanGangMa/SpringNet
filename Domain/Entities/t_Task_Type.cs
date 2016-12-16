namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Task_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Task_Type()
        {
            t_Tasks = new HashSet<t_Tasks>();
        }

        [Key]
        public Guid s_Task_TypeID { get; set; }

        public int s_Task_DeptID { get; set; }

        [StringLength(20)]
        public string s_Task_TypeName { get; set; }

        public byte? s_Task_TypeSort { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Tasks> t_Tasks { get; set; }
    }
}
