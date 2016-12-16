namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Users()
        {
            t_MonthlyCustomer = new HashSet<t_MonthlyCustomer>();
            t_Orders = new HashSet<t_Orders>();
            t_OrderTicket = new HashSet<t_OrderTicket>();
            t_OrderTracker = new HashSet<t_OrderTracker>();
            t_Task_Contacts = new HashSet<t_Task_Contacts>();
            t_Task_Group_User_Ref = new HashSet<t_Task_Group_User_Ref>();
        }

        [Key]
        [StringLength(20)]
        public string s_UserID { get; set; }

        [StringLength(10)]
        public string s_TrueName { get; set; }

        [StringLength(30)]
        public string s_UserPwd { get; set; }

        public int s_DeptID { get; set; }

        public byte s_IsLock { get; set; }

        [StringLength(4)]
        public string s_Sex { get; set; }

        [StringLength(300)]
        public string s_ImgIco { get; set; }

        public DateTime? s_AddTime { get; set; }

        [StringLength(10)]
        public string s_AddUser { get; set; }

        public DateTime? s_EditTime { get; set; }

        [StringLength(10)]
        public string s_EditUser { get; set; }

        public virtual t_Department t_Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_MonthlyCustomer> t_MonthlyCustomer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Orders> t_Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_OrderTicket> t_OrderTicket { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_OrderTracker> t_OrderTracker { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Task_Contacts> t_Task_Contacts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Task_Group_User_Ref> t_Task_Group_User_Ref { get; set; }
    }
}
