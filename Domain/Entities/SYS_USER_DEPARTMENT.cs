namespace Domain
{
    using System.ComponentModel.DataAnnotations;

    public partial class SYS_USER_DEPARTMENT
    {
        public int ID { get; set; }

        public int USER_ID { get; set; }

        [Required]
        [StringLength(36)]
        public string DEPARTMENT_ID { get; set; }

        public virtual SYS_DEPARTMENT SYS_DEPARTMENT { get; set; }

        public virtual SYS_USER SYS_USER { get; set; }
    }
}
