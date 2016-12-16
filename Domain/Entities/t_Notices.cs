namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Notices
    {
        [Key]
        public Guid s_NoticeID { get; set; }

        public string s_NoticeTitle { get; set; }

        public string s_NoticeContent { get; set; }

        public DateTime s_AddTime { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }
    }
}
