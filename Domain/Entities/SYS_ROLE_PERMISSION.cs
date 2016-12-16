namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_ROLE_PERMISSION
    {
        public int ID { get; set; }

        public int ROLEID { get; set; }

        public int PERMISSIONID { get; set; }

        public virtual SYS_PERMISSION SYS_PERMISSION { get; set; }

        public virtual SYS_ROLE SYS_ROLE { get; set; }
    }
}
