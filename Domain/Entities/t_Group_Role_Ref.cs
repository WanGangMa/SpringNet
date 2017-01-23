namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_Group_Role_Ref
    {
        [Key]
        public int s_RGR_ID { get; set; }

        public Guid s_RoleCode { get; set; }

        public Guid s_GroupCode { get; set; }
    }
}
