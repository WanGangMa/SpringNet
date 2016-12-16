namespace Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_CustomerContact
    {
        [Key]
        public int s_ContactID { get; set; }

        [StringLength(30)]
        public string s_CustomerID { get; set; }

        [Required]
        [StringLength(20)]
        public string s_ContactName { get; set; }

        [Required]
        [StringLength(20)]
        public string s_ContactPhone { get; set; }

        [StringLength(30)]
        public string s_ContactEmail { get; set; }

        [Required]
        [StringLength(10)]
        public string s_ContactDuty { get; set; }

        public byte s_ContactType { get; set; }

        [StringLength(50)]
        public string s_Remark { get; set; }

        public DateTime s_AddTime { get; set; }

        public virtual t_Customers t_Customers { get; set; }
    }
}
