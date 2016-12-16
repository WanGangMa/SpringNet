namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_RolePermission
    {
        [Key]
        public Guid s_RolePermissionID { get; set; }

        public Guid s_RoleCode { get; set; }

        [StringLength(50)]
        public string s_RolePermissionRule { get; set; }

        public byte s_RolePermissionType { get; set; }

        public virtual t_UserRole t_UserRole { get; set; }
    }
}
