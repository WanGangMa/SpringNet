namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class t_Task_Quotes
    {
        [Key]
        public Guid s_Task_QuoteID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_TaskID { get; set; }

        [StringLength(50)]
        public string s_Task_QuoteKey { get; set; }

        public byte s_Task_QuoteType { get; set; }

        public DateTime s_AddTime { get; set; }

        public virtual t_Tasks t_Tasks { get; set; }
    }
}
