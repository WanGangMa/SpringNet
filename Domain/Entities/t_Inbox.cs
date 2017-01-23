namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_Inbox
    {
        [Key]
        public Guid s_MessageID { get; set; }

        [Required]
        [StringLength(100)]
        public string s_Subject { get; set; }

        [StringLength(800)]
        public string s_Content { get; set; }

        [Required]
        [StringLength(20)]
        public string s_Recipient { get; set; }

        public DateTime? s_ReadTime { get; set; }

        [StringLength(20)]
        public string s_Sender { get; set; }

        public DateTime s_SendTime { get; set; }

        public byte s_MessageType { get; set; }

        public byte s_MessageState { get; set; }

        public byte s_IsUrgentMail { get; set; }

        public byte s_InboxState { get; set; }

        public byte s_OutboxState { get; set; }
    }
}
