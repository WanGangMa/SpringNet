namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Group_Role_Ref
    {
        [Key]
        public int s_RGR_ID { get; set; }

        public Guid s_RoleCode { get; set; }

        public Guid s_GroupCode { get; set; }
    }
}
