namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_UserGroup
    {
        [Key]
        public Guid s_GroupCode { get; set; }

        public int s_GroupID { get; set; }

        [Required]
        [StringLength(30)]
        public string s_GroupName { get; set; }

        public DateTime? s_AddTime { get; set; }

        [StringLength(10)]
        public string s_AddUser { get; set; }

        public DateTime? s_EditTime { get; set; }

        [StringLength(10)]
        public string s_EditUser { get; set; }

        [StringLength(20)]
        public string s_GroupRemark { get; set; }

        public int s_ParentID { get; set; }
    }
}
