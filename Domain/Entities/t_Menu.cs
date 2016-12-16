namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Menu
    {
        [Key]
        public Guid s_MenuCode { get; set; }

        [Required]
        [StringLength(8000)]
        public string s_MenuID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_MenuName { get; set; }

        [StringLength(100)]
        public string s_MenuURL { get; set; }

        [StringLength(50)]
        public string s_MenuICO { get; set; }

        [Required]
        [StringLength(8000)]
        public string s_ParentID { get; set; }

        public int s_MenuLevel { get; set; }

        public int s_MenuSort { get; set; }

        public DateTime? s_AddTime { get; set; }

        public byte s_IsEnable { get; set; }
    }
}
