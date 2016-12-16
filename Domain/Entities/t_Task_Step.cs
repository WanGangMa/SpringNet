namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Task_Step
    {
        [Key]
        public Guid s_Task_StepID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_TaskID { get; set; }

        [Required]
        [StringLength(1000)]
        public string s_Content { get; set; }

        [StringLength(20)]
        public string s_AddUser { get; set; }

        public DateTime? s_AddTime { get; set; }

        [StringLength(20)]
        public string s_ReferTo { get; set; }

        public byte s_IsRead { get; set; }

        public DateTime? s_ReadTime { get; set; }

        public virtual t_Tasks t_Tasks { get; set; }
    }
}
