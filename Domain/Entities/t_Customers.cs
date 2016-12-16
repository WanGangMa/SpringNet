namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class CustomerCompare : IEqualityComparer<t_Customers>
    {
        public bool Equals(t_Customers x, t_Customers y)
        {
            return (x.s_CustomerID == y.s_CustomerID && x.s_CustomerName == y.s_CustomerName);
        }

        public int GetHashCode(t_Customers obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var checkin = obj.s_CustomerID + obj.s_CustomerName;
                return checkin.ToString().GetHashCode();
            }
        }
    }

    public partial class t_Customers
    {
        public override int GetHashCode()
        {
            return (this.s_CustomerID + this.s_CustomerName).ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return (obj.GetType().GetProperty("s_CustomerID").GetValue(obj,null).ToString() ==this.s_CustomerID) 
                    &&(obj.GetType().GetProperty("s_CustomerName").GetValue(obj, null).ToString() == this.s_CustomerName);
        }
       

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Customers()
        {
            t_Bills = new HashSet<t_Bills>();
            t_CustomerContact = new HashSet<t_CustomerContact>();
            t_MonthlyStatement = new HashSet<t_MonthlyStatement>();
            t_Orders = new HashSet<t_Orders>();
            t_Tasks = new HashSet<t_Tasks>();
            t_TransactionRecord = new HashSet<t_TransactionRecord>();
        }

        [Key]
        [StringLength(30)]
        public string s_CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string s_CustomerName { get; set; }

        [StringLength(30)]
        public string s_Email { get; set; }

        [StringLength(20)]
        public string s_Telephone { get; set; }

        [StringLength(20)]
        public string s_Fax { get; set; }

        public Guid? s_CustomerSourceID { get; set; }

        public Guid? s_CustomerIndustryID { get; set; }

        public Guid? s_CustomerPropertyID { get; set; }

        public int? s_Province { get; set; }

        public int? s_City { get; set; }

        public int? s_County { get; set; }

        [StringLength(100)]
        public string s_Address { get; set; }

        public byte s_CustomerState { get; set; }

        [Column(TypeName = "money")]
        public decimal s_AccountBalance { get; set; }

        public DateTime s_AddTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Bills> t_Bills { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_CustomerContact> t_CustomerContact { get; set; }

        public virtual t_CustomerIndustry t_CustomerIndustry { get; set; }

        public virtual t_CustomerProperty t_CustomerProperty { get; set; }

        public virtual t_CustomerSource t_CustomerSource { get; set; }

        public virtual t_MonthlyCustomer t_MonthlyCustomer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_MonthlyStatement> t_MonthlyStatement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Orders> t_Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_Tasks> t_Tasks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_TransactionRecord> t_TransactionRecord { get; set; }
    }
}
