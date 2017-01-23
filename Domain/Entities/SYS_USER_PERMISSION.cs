namespace Domain
{
    public partial class SYS_USER_PERMISSION
    {
        public int ID { get; set; }

        public int FK_USERID { get; set; }

        public int FK_PERMISSIONID { get; set; }

        public virtual SYS_PERMISSION SYS_PERMISSION { get; set; }

        public virtual SYS_USER SYS_USER { get; set; }
    }
}
