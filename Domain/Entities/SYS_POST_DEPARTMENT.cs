namespace Domain
{
    using System.ComponentModel.DataAnnotations;

    public partial class SYS_POST_DEPARTMENT
    {
        public int ID { get; set; }

        [Required]
        [StringLength(36)]
        public string FK_DEPARTMENT_ID { get; set; }

        [Required]
        [StringLength(36)]
        public string FK_POST_ID { get; set; }

        public virtual SYS_DEPARTMENT SYS_DEPARTMENT { get; set; }

        public virtual SYS_POST SYS_POST { get; set; }
    }
}
