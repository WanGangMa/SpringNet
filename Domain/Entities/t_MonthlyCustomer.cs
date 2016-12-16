namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_MonthlyCustomer
    {
        [Key]
        [StringLength(30)]
        public string s_MonthlyID { get; set; }

        [StringLength(5)]
        public string s_CreditLevel { get; set; }

        [Column(TypeName = "money")]
        public decimal? s_CreditLine { get; set; }

        public int s_AccountDay { get; set; }

        public byte s_IsAccess { get; set; }

        [StringLength(20)]
        public string s_UserID { get; set; }

        public DateTime s_AddTime { get; set; }

        public virtual t_Customers t_Customers { get; set; }

        public virtual t_Users t_Users { get; set; }
    }
}
