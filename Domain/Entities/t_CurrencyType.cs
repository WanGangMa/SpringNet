namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class t_CurrencyType
    {
        [Key]
        [StringLength(10)]
        public string s_CurrencyVal { get; set; }

        [StringLength(10)]
        public string s_CurrencyName { get; set; }

        [StringLength(10)]
        public string s_CurrencyUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? s_CurrencyExchange { get; set; }
    }
}
