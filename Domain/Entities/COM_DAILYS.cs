namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class COM_DAILYS
    {
        public int ID { get; set; }

        public int FK_USERID { get; set; }

        [StringLength(72)]
        public string FK_RELATIONID { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime LastEditDate { get; set; }

        [Required]
        [StringLength(50)]
        public string DailySubIP { get; set; }
    }
}
