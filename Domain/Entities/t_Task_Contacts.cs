namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Task_Contacts
    {
        [Key]
        public Guid s_Task_ContactID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_TaskID { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }

        public virtual t_Tasks t_Tasks { get; set; }

        public virtual t_Users t_Users { get; set; }
    }
}
