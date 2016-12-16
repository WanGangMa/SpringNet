namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Task_Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Task_Group()
        {
            t_Task_Group_User_Ref = new HashSet<t_Task_Group_User_Ref>();
            t_Tasks = new HashSet<t_Tasks>();
        }

        [Key]
        public Guid s_Task_GroupID { get; set; }

        public int s_Task_DeptID { get; set; }

        [StringLength(200)]
        public string s_Task_GroupName { get; set; }

        [StringLength(30)]
        public string s_Task_GroupName_EN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Task_Group_User_Ref> t_Task_Group_User_Ref { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Tasks> t_Tasks { get; set; }
    }
}
