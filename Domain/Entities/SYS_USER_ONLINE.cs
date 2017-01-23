namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class SYS_USER_ONLINE
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string ConnectId { get; set; }

        public int FK_UserId { get; set; }

        public DateTime OnlineDate { get; set; }

        public DateTime? OfflineDate { get; set; }

        public bool IsOnline { get; set; }

        [Required]
        [StringLength(50)]
        public string UserIP { get; set; }

        public virtual SYS_USER SYS_USER { get; set; }
    }
}
