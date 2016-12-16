namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_POST_USER
    {
        public int ID { get; set; }

        public int FK_USERID { get; set; }

        [Required]
        [StringLength(36)]
        public string FK_POSTID { get; set; }

        public virtual SYS_USER SYS_USER { get; set; }

        public virtual SYS_POST SYS_POST { get; set; }
    }
}
