namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_Task_Attachments
    {
        [Key]
        public Guid s_Task_AttachmentID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_TaskID { get; set; }

        [StringLength(100)]
        public string s_AttachmentName { get; set; }

        [StringLength(200)]
        public string s_AttachmentPath { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }

        public DateTime? s_AddTime { get; set; }

        public byte? s_AttachmentState { get; set; }

        public virtual t_Tasks t_Tasks { get; set; }
    }
}
