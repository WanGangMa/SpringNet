namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_UserRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_UserRole()
        {
            t_RolePermission = new HashSet<t_RolePermission>();
        }

        [Key]
        public Guid s_RoleCode { get; set; }

        public int s_RoleID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_RoleName { get; set; }

        public DateTime? s_AddTime { get; set; }

        [StringLength(10)]
        public string s_AddUser { get; set; }

        public DateTime? s_EditTime { get; set; }

        [StringLength(10)]
        public string s_EditUser { get; set; }

        [StringLength(20)]
        public string s_RoleRemark { get; set; }

        [StringLength(50)]
        public string s_RoleDescription { get; set; }

        public int s_ParentID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_RolePermission> t_RolePermission { get; set; }
    }
}
