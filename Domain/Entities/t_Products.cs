namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class t_Products
    {
        [Key]
        [StringLength(25)]
        public string s_ProductID { get; set; }

        [StringLength(20)]
        public string s_ProductName { get; set; }

        [StringLength(10)]
        public string s_Unit { get; set; }

        [Column(TypeName = "money")]
        public decimal s_Price { get; set; }

        public double? s_Weight { get; set; }

        [Column(TypeName = "money")]
        public decimal? s_Discount { get; set; }

        [StringLength(100)]
        public string s_Describe { get; set; }

        public int s_Repertory { get; set; }
    }
}
