namespace Domain
{
    using System.ComponentModel.DataAnnotations;

    public partial class SYS_CODE_AREA
    {
        [StringLength(50)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string PID { get; set; }

        [StringLength(200)]
        public string NAME { get; set; }

        public byte LEVELS { get; set; }
    }
}
