namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_OrderTracker
    {
        [Key]
        public Guid s_TrackerID { get; set; }

        [Required]
        [StringLength(30)]
        public string s_OrderID { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }

        public DateTime s_Addtime { get; set; }

        [StringLength(500)]
        public string s_Description { get; set; }

        [StringLength(20)]
        public string s_ReferTo { get; set; }

        public virtual t_Orders t_Orders { get; set; }

        public virtual t_Users t_Users { get; set; }
    }
}
