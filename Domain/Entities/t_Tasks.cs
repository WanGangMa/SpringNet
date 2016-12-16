namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Tasks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Tasks()
        {
            t_Task_Attachments = new HashSet<t_Task_Attachments>();
            t_Task_Contacts = new HashSet<t_Task_Contacts>();
            t_Task_Quotes = new HashSet<t_Task_Quotes>();
            t_Task_Step = new HashSet<t_Task_Step>();
        }

        [Key]
        [StringLength(20)]
        public string s_TaskID { get; set; }

        [StringLength(50)]
        public string s_RelationEntity { get; set; }

        [StringLength(50)]
        public string s_RelationEntityID { get; set; }

        [StringLength(50)]
        public string s_RelationEntityValue { get; set; }

        public Guid s_Task_TypeID { get; set; }

        public Guid s_Task_GroupID { get; set; }

        [StringLength(30)]
        public string s_CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string s_Relative { get; set; }

        [Required]
        [StringLength(100)]
        public string s_Subject { get; set; }

        [Required]
        [StringLength(20)]
        public string s_Sponsor { get; set; }

        [Required]
        [StringLength(20)]
        public string s_Operator { get; set; }

        public DateTime s_AddTime { get; set; }

        public DateTime? s_EditTime { get; set; }

        public Guid s_Task_StateID { get; set; }

        [StringLength(50)]
        public string s_RelationDataRoute { get; set; }

        public byte s_RelationEntityType { get; set; }

        public virtual t_Customers t_Customers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Task_Attachments> t_Task_Attachments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Task_Contacts> t_Task_Contacts { get; set; }

        public virtual t_Task_Group t_Task_Group { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Task_Quotes> t_Task_Quotes { get; set; }

        public virtual t_Task_State t_Task_State { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Task_Step> t_Task_Step { get; set; }

        public virtual t_Task_Type t_Task_Type { get; set; }
    }
}
