namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class t_PlaceInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int s_PlaceID { get; set; }

        [StringLength(30)]
        public string s_PlaceName { get; set; }

        public byte? s_PlaceLevel { get; set; }

        public int? s_ParentID { get; set; }
    }
}
