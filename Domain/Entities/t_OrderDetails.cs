namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_OrderDetails
    {
        [Key]
        public Guid s_OrderDetailsID { get; set; }

        [Required]
        [StringLength(30)]
        public string s_OrderID { get; set; }

        [StringLength(20)]
        public string s_ProductName { get; set; }

        public int s_ProductCount { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Price { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Discount { get; set; }

        [StringLength(100)]
        public string s_Describe { get; set; }

        public virtual t_Orders t_Orders { get; set; }
    }
}
