namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Orders()
        {
            t_OrderDetails = new HashSet<t_OrderDetails>();
            t_OrderTracker = new HashSet<t_OrderTracker>();
        }

        [Key]
        [StringLength(30)]
        public string s_OrderID { get; set; }

        [StringLength(20)]
        public string s_TaskID { get; set; }

        [StringLength(50)]
        public string s_BillID { get; set; }

        [Required]
        [StringLength(30)]
        public string s_CustomerID { get; set; }

        public DateTime s_OrderTime { get; set; }

        public DateTime? s_EditTime { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }

        [StringLength(20)]
        public string s_Approver { get; set; }

        [StringLength(20)]
        public string s_Picker { get; set; }

        [StringLength(100)]
        public string s_PickFile { get; set; }

        [StringLength(20)]
        public string s_Shipper { get; set; }

        [StringLength(100)]
        public string s_ShipFile { get; set; }

        [StringLength(20)]
        public string s_Recipient { get; set; }

        [StringLength(100)]
        public string s_ReceiveFile { get; set; }

        [Column(TypeName = "money")]
        public decimal s_ProPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Taxes { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Freight { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Insurance { get; set; }

        public byte? s_CurrencyType { get; set; }

        [Column(TypeName = "money")]
        public decimal s_TotalMoney { get; set; }

        public byte s_OrderType { get; set; }

        public byte s_OrderState { get; set; }

        public byte s_FundState { get; set; }

        public byte s_LogisticalState { get; set; }

        public byte s_PayType { get; set; }

        public byte s_IsMonthlyloan { get; set; }

        public byte s_ReceivedPayment { get; set; }

        public byte s_ShipmentConfirmation { get; set; }

        public byte s_IssuedBill { get; set; }

        [Required]
        [StringLength(50)]
        public string s_Delivery { get; set; }

        [Required]
        [StringLength(50)]
        public string s_Receiving { get; set; }

        public DateTime? s_Validity { get; set; }

        [StringLength(100)]
        public string s_Remark { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Guid? s_TicketID { get; set; }

        public virtual t_Bills t_Bills { get; set; }

        public virtual t_Customers t_Customers { get; set; }

        public virtual t_FundState t_FundState { get; set; }

        public virtual t_LogisticalState t_LogisticalState { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_OrderDetails> t_OrderDetails { get; set; }

        public virtual t_OrderState t_OrderState { get; set; }

        public virtual t_OrderTicket t_OrderTicket { get; set; }

        public virtual t_PayType t_PayType { get; set; }

        public virtual t_Users t_Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_OrderTracker> t_OrderTracker { get; set; }
    }
}
