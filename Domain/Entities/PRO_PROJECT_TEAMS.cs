namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class PRO_PROJECT_TEAMS
    {
        public int ID { get; set; }

        public int FK_StageId { get; set; }

        public int FK_UserId { get; set; }

        [StringLength(300)]
        public string ApplyReasons { get; set; }

        public int JionStatus { get; set; }

        [StringLength(300)]
        public string RefuseReasons { get; set; }

        public DateTime JionDate { get; set; }

        public virtual PRO_PROJECT_STAGES PRO_PROJECT_STAGES { get; set; }
    }
}
