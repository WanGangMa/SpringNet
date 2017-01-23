namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_User_Group_Ref
    {
        [Key]
        public int s_UGR_ID { get; set; }

        [StringLength(30)]
        public string s_UserID { get; set; }

        public Guid s_GroupCode { get; set; }
    }
}
