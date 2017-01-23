namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_Buttons
    {
        [Key]
        public Guid s_ButtonID { get; set; }

        [StringLength(50)]
        public string s_ButtonICO { get; set; }

        [StringLength(50)]
        public string s_ButtonText { get; set; }

        public byte s_ButtonSort { get; set; }

        public byte s_ButtonLevel { get; set; }

        public Guid s_MenuCode { get; set; }

        [StringLength(50)]
        public string s_ButtonClass { get; set; }

        [StringLength(300)]
        public string s_ButtonAttribute { get; set; }

        [StringLength(300)]
        public string s_ClickEvent { get; set; }

        public byte s_isEnable { get; set; }
    }
}
