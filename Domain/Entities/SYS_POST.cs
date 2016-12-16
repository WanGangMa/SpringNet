namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_POST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SYS_POST()
        {
            SYS_POST_DEPARTMENT = new HashSet<SYS_POST_DEPARTMENT>();
        }

        [StringLength(36)]
        public string ID { get; set; }

        [Required]
        [StringLength(36)]
        public string FK_DEPARTID { get; set; }

        [StringLength(100)]
        public string POSTNAME { get; set; }

        [Required]
        [StringLength(36)]
        public string POSTTYPE { get; set; }

        [StringLength(500)]
        public string REMARK { get; set; }

        public int? SHOWORDER { get; set; }

        [StringLength(50)]
        public string CREATEUSER { get; set; }

        public DateTime CREATEDATE { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        [StringLength(36)]
        public string UPDATEUSER { get; set; }

        public virtual SYS_DEPARTMENT SYS_DEPARTMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SYS_POST_DEPARTMENT> SYS_POST_DEPARTMENT { get; set; }
    }
}
