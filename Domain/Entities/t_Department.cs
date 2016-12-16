namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Department()
        {
            t_Users = new HashSet<t_Users>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int s_DeptID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_DeptName { get; set; }

        [StringLength(20)]
        public string s_DeptRemark { get; set; }

        public DateTime? s_AddTime { get; set; }

        [StringLength(10)]
        public string s_AddUser { get; set; }

        public DateTime? s_EditTime { get; set; }

        [StringLength(10)]
        public string s_EditUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Users> t_Users { get; set; }
    }
}
