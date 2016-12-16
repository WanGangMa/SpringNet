namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_User_Role_Ref
    {
        [Key]
        public int s_URRID { get; set; }

        public Guid s_RoleCode { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }
    }
}
