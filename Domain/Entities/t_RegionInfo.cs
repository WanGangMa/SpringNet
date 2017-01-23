namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class t_RegionInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int s_RegionID { get; set; }

        public string s_RegionName { get; set; }

        public int s_RegionParentID { get; set; }
    }
}
