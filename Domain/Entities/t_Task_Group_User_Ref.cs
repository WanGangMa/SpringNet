namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Task_Group_User_Ref
    {
        [Key]
        public Guid s_Task_Group_UserID { get; set; }

        public Guid s_Task_GroupID { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }

        public byte s_IsGroupLeader { get; set; }

        public virtual t_Task_Group t_Task_Group { get; set; }

        public virtual t_Users t_Users { get; set; }
    }
}
