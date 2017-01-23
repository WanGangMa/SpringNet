namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_Sys_Log
    {
        [Key]
        public Guid s_LogID { get; set; }

        public DateTime s_Date { get; set; }

        [StringLength(20)]
        public string s_Level { get; set; }

        [StringLength(200)]
        public string s_Logger { get; set; }

        [StringLength(100)]
        public string s_ClientUser { get; set; }

        [StringLength(20)]
        public string s_ClientIP { get; set; }

        [StringLength(500)]
        public string s_RequestURl { get; set; }

        [StringLength(20)]
        public string s_Action { get; set; }

        [StringLength(4000)]
        public string s_Message { get; set; }

        [StringLength(4000)]
        public string s_Exception { get; set; }
    }
}
