namespace Domain
{
    using System.Data.Entity;

    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }

        public virtual DbSet<COM_CONTENT> COM_CONTENT { get; set; }
        public virtual DbSet<COM_DAILYS> COM_DAILYS { get; set; }
        public virtual DbSet<COM_UPLOAD> COM_UPLOAD { get; set; }
        public virtual DbSet<COM_WORKATTENDANCE> COM_WORKATTENDANCE { get; set; }
        public virtual DbSet<MAIL_ATTACHMENT> MAIL_ATTACHMENT { get; set; }
        public virtual DbSet<MAIL_INBOX> MAIL_INBOX { get; set; }
        public virtual DbSet<MAIL_OUTBOX> MAIL_OUTBOX { get; set; }
        public virtual DbSet<PRO_PROJECT_FILES> PRO_PROJECT_FILES { get; set; }
        public virtual DbSet<PRO_PROJECT_MESSAGE> PRO_PROJECT_MESSAGE { get; set; }
        public virtual DbSet<PRO_PROJECT_STAGES> PRO_PROJECT_STAGES { get; set; }
        public virtual DbSet<PRO_PROJECT_TEAMS> PRO_PROJECT_TEAMS { get; set; }
        public virtual DbSet<PRO_PROJECTS> PRO_PROJECTS { get; set; }
        public virtual DbSet<SYS_BUSSINESSCUSTOMER> SYS_BUSSINESSCUSTOMER { get; set; }
        public virtual DbSet<SYS_CHATMESSAGE> SYS_CHATMESSAGE { get; set; }
        public virtual DbSet<SYS_CODE> SYS_CODE { get; set; }
        public virtual DbSet<SYS_CODE_AREA> SYS_CODE_AREA { get; set; }
        public virtual DbSet<SYS_DEPARTMENT> SYS_DEPARTMENT { get; set; }
        public virtual DbSet<SYS_LOG> SYS_LOG { get; set; }
        public virtual DbSet<SYS_MODULE> SYS_MODULE { get; set; }
        public virtual DbSet<SYS_PERMISSION> SYS_PERMISSION { get; set; }
        public virtual DbSet<SYS_POST> SYS_POST { get; set; }
        public virtual DbSet<SYS_POST_DEPARTMENT> SYS_POST_DEPARTMENT { get; set; }
        public virtual DbSet<SYS_POST_USER> SYS_POST_USER { get; set; }
        public virtual DbSet<SYS_ROLE> SYS_ROLE { get; set; }
        public virtual DbSet<SYS_ROLE_PERMISSION> SYS_ROLE_PERMISSION { get; set; }
        public virtual DbSet<SYS_SYSTEM> SYS_SYSTEM { get; set; }
        public virtual DbSet<SYS_USER> SYS_USER { get; set; }
        public virtual DbSet<SYS_USER_DEPARTMENT> SYS_USER_DEPARTMENT { get; set; }
        public virtual DbSet<SYS_USER_ONLINE> SYS_USER_ONLINE { get; set; }
        public virtual DbSet<SYS_USER_PERMISSION> SYS_USER_PERMISSION { get; set; }
        public virtual DbSet<SYS_USER_ROLE> SYS_USER_ROLE { get; set; }
        public virtual DbSet<SYS_USERINFO> SYS_USERINFO { get; set; }
        public virtual DbSet<t_Bills> t_Bills { get; set; }
        public virtual DbSet<t_BillState> t_BillState { get; set; }
        public virtual DbSet<t_Buttons> t_Buttons { get; set; }
        public virtual DbSet<t_CurrencyType> t_CurrencyType { get; set; }
        public virtual DbSet<t_CustomerContact> t_CustomerContact { get; set; }
        public virtual DbSet<t_CustomerIndustry> t_CustomerIndustry { get; set; }
        public virtual DbSet<t_CustomerProperty> t_CustomerProperty { get; set; }
        public virtual DbSet<t_Customers> t_Customers { get; set; }
        public virtual DbSet<t_CustomerSource> t_CustomerSource { get; set; }
        public virtual DbSet<t_Department> t_Department { get; set; }
        public virtual DbSet<t_FundState> t_FundState { get; set; }
        public virtual DbSet<t_Group_Role_Ref> t_Group_Role_Ref { get; set; }
        public virtual DbSet<t_Inbox> t_Inbox { get; set; }
        public virtual DbSet<t_LogisticalState> t_LogisticalState { get; set; }
        public virtual DbSet<t_Menu> t_Menu { get; set; }
        public virtual DbSet<t_MonthlyCustomer> t_MonthlyCustomer { get; set; }
        public virtual DbSet<t_MonthlyStatement> t_MonthlyStatement { get; set; }
        public virtual DbSet<t_Notices> t_Notices { get; set; }
        public virtual DbSet<t_OrderDetails> t_OrderDetails { get; set; }
        public virtual DbSet<t_Orders> t_Orders { get; set; }
        public virtual DbSet<t_OrderState> t_OrderState { get; set; }
        public virtual DbSet<t_OrderTicket> t_OrderTicket { get; set; }
        public virtual DbSet<t_OrderTracker> t_OrderTracker { get; set; }
        public virtual DbSet<t_PayType> t_PayType { get; set; }
        public virtual DbSet<t_PlaceInfo> t_PlaceInfo { get; set; }
        public virtual DbSet<t_Products> t_Products { get; set; }
        public virtual DbSet<t_RegionInfo> t_RegionInfo { get; set; }
        public virtual DbSet<t_RolePermission> t_RolePermission { get; set; }
        public virtual DbSet<t_Sys_Log> t_Sys_Log { get; set; }
        public virtual DbSet<t_Task_Attachments> t_Task_Attachments { get; set; }
        public virtual DbSet<t_Task_Contacts> t_Task_Contacts { get; set; }
        public virtual DbSet<t_Task_Department> t_Task_Department { get; set; }
        public virtual DbSet<t_Task_Group> t_Task_Group { get; set; }
        public virtual DbSet<t_Task_Group_User_Ref> t_Task_Group_User_Ref { get; set; }
        public virtual DbSet<t_Task_Quotes> t_Task_Quotes { get; set; }
        public virtual DbSet<t_Task_State> t_Task_State { get; set; }
        public virtual DbSet<t_Task_Step> t_Task_Step { get; set; }
        public virtual DbSet<t_Task_Type> t_Task_Type { get; set; }
        public virtual DbSet<t_Tasks> t_Tasks { get; set; }
        public virtual DbSet<t_TransactionRecord> t_TransactionRecord { get; set; }
        public virtual DbSet<t_User_Group_Ref> t_User_Group_Ref { get; set; }
        public virtual DbSet<t_User_Role_Ref> t_User_Role_Ref { get; set; }
        public virtual DbSet<t_UserGroup> t_UserGroup { get; set; }
        public virtual DbSet<t_UserRole> t_UserRole { get; set; }
        public virtual DbSet<t_Users> t_Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MAIL_OUTBOX>()
                .HasMany(e => e.MAIL_ATTACHMENT)
                .WithRequired(e => e.MAIL_OUTBOX)
                .HasForeignKey(e => e.FK_MailID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MAIL_OUTBOX>()
                .HasMany(e => e.MAIL_INBOX)
                .WithRequired(e => e.MAIL_OUTBOX)
                .HasForeignKey(e => e.FK_MailID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRO_PROJECT_STAGES>()
                .HasMany(e => e.PRO_PROJECT_TEAMS)
                .WithRequired(e => e.PRO_PROJECT_STAGES)
                .HasForeignKey(e => e.FK_StageId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRO_PROJECTS>()
                .HasMany(e => e.PRO_PROJECT_MESSAGE)
                .WithRequired(e => e.PRO_PROJECTS)
                .HasForeignKey(e => e.FK_ProjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRO_PROJECTS>()
                .HasMany(e => e.PRO_PROJECT_STAGES)
                .WithRequired(e => e.PRO_PROJECTS)
                .HasForeignKey(e => e.FK_ProjectId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_CODE_AREA>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_CODE_AREA>()
                .Property(e => e.PID)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_DEPARTMENT>()
                .HasMany(e => e.SYS_POST_DEPARTMENT)
                .WithRequired(e => e.SYS_DEPARTMENT)
                .HasForeignKey(e => e.FK_DEPARTMENT_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_DEPARTMENT>()
                .HasMany(e => e.SYS_POST)
                .WithRequired(e => e.SYS_DEPARTMENT)
                .HasForeignKey(e => e.FK_DEPARTID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_DEPARTMENT>()
                .HasMany(e => e.SYS_USER_DEPARTMENT)
                .WithRequired(e => e.SYS_DEPARTMENT)
                .HasForeignKey(e => e.DEPARTMENT_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_MODULE>()
                .HasMany(e => e.SYS_PERMISSION)
                .WithRequired(e => e.SYS_MODULE)
                .HasForeignKey(e => e.MODULEID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_PERMISSION>()
                .HasMany(e => e.SYS_ROLE_PERMISSION)
                .WithRequired(e => e.SYS_PERMISSION)
                .HasForeignKey(e => e.PERMISSIONID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_PERMISSION>()
                .HasMany(e => e.SYS_USER_PERMISSION)
                .WithRequired(e => e.SYS_PERMISSION)
                .HasForeignKey(e => e.FK_PERMISSIONID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_POST>()
                .HasMany(e => e.SYS_POST_DEPARTMENT)
                .WithRequired(e => e.SYS_POST)
                .HasForeignKey(e => e.FK_POST_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_ROLE>()
                .HasMany(e => e.SYS_ROLE_PERMISSION)
                .WithRequired(e => e.SYS_ROLE)
                .HasForeignKey(e => e.ROLEID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_ROLE>()
                .HasMany(e => e.SYS_USER_ROLE)
                .WithRequired(e => e.SYS_ROLE)
                .HasForeignKey(e => e.FK_ROLEID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_SYSTEM>()
                .HasMany(e => e.SYS_MODULE)
                .WithRequired(e => e.SYS_SYSTEM)
                .HasForeignKey(e => e.FK_BELONGSYSTEM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_USER>()
                .HasMany(e => e.SYS_POST_USER)
                .WithRequired(e => e.SYS_USER)
                .HasForeignKey(e => e.FK_USERID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_USER>()
                .HasMany(e => e.SYS_USER_DEPARTMENT)
                .WithRequired(e => e.SYS_USER)
                .HasForeignKey(e => e.USER_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_USER>()
                .HasMany(e => e.SYS_USER_ONLINE)
                .WithRequired(e => e.SYS_USER)
                .HasForeignKey(e => e.FK_UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_USER>()
                .HasMany(e => e.SYS_USER_PERMISSION)
                .WithRequired(e => e.SYS_USER)
                .HasForeignKey(e => e.FK_USERID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_USER>()
                .HasMany(e => e.SYS_USER_ROLE)
                .WithRequired(e => e.SYS_USER)
                .HasForeignKey(e => e.FK_USERID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SYS_USER>()
                .HasMany(e => e.SYS_USERINFO)
                .WithRequired(e => e.SYS_USER)
                .HasForeignKey(e => e.USERID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_Bills>()
                .Property(e => e.s_BillID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Bills>()
                .Property(e => e.s_CustomerID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Bills>()
                .Property(e => e.s_Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Bills>()
                .Property(e => e.s_AddUser)
                .IsUnicode(false);

            modelBuilder.Entity<t_Bills>()
                .Property(e => e.s_EditUser)
                .IsUnicode(false);

            modelBuilder.Entity<t_Bills>()
                .Property(e => e.s_BillItems)
                .IsUnicode(false);

            modelBuilder.Entity<t_Buttons>()
                .Property(e => e.s_ButtonICO)
                .IsUnicode(false);

            modelBuilder.Entity<t_CurrencyType>()
                .Property(e => e.s_CurrencyExchange)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_CustomerContact>()
                .Property(e => e.s_CustomerID)
                .IsUnicode(false);

            modelBuilder.Entity<t_CustomerContact>()
                .Property(e => e.s_ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<t_Customers>()
                .Property(e => e.s_CustomerID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Customers>()
                .Property(e => e.s_Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<t_Customers>()
                .Property(e => e.s_Fax)
                .IsUnicode(false);

            modelBuilder.Entity<t_Customers>()
                .Property(e => e.s_AccountBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Customers>()
                .HasOptional(e => e.t_MonthlyCustomer)
                .WithRequired(e => e.t_Customers);

            modelBuilder.Entity<t_Department>()
                .Property(e => e.s_DeptName)
                .IsUnicode(false);

            modelBuilder.Entity<t_Inbox>()
                .Property(e => e.s_Recipient)
                .IsUnicode(false);

            modelBuilder.Entity<t_Inbox>()
                .Property(e => e.s_Sender)
                .IsUnicode(false);

            modelBuilder.Entity<t_Menu>()
                .Property(e => e.s_MenuID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Menu>()
                .Property(e => e.s_MenuURL)
                .IsUnicode(false);

            modelBuilder.Entity<t_Menu>()
                .Property(e => e.s_MenuICO)
                .IsUnicode(false);

            modelBuilder.Entity<t_Menu>()
                .Property(e => e.s_ParentID)
                .IsUnicode(false);

            modelBuilder.Entity<t_MonthlyCustomer>()
                .Property(e => e.s_MonthlyID)
                .IsUnicode(false);

            modelBuilder.Entity<t_MonthlyCustomer>()
                .Property(e => e.s_CreditLevel)
                .IsUnicode(false);

            modelBuilder.Entity<t_MonthlyCustomer>()
                .Property(e => e.s_CreditLine)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_MonthlyCustomer>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_MonthlyStatement>()
                .Property(e => e.s_CustomerID)
                .IsUnicode(false);

            modelBuilder.Entity<t_MonthlyStatement>()
                .Property(e => e.s_LastBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_MonthlyStatement>()
                .Property(e => e.s_CurrentPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_MonthlyStatement>()
                .Property(e => e.s_CurrentPaid)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_MonthlyStatement>()
                .Property(e => e.s_CurrentBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_MonthlyStatement>()
                .Property(e => e.s_Arrears)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_MonthlyStatement>()
                .HasMany(e => e.t_TransactionRecord)
                .WithOptional(e => e.t_MonthlyStatement)
                .HasForeignKey(e => e.t_MonthlyStatement_s_StatementID);

            modelBuilder.Entity<t_Notices>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_OrderDetails>()
                .Property(e => e.s_OrderID)
                .IsUnicode(false);

            modelBuilder.Entity<t_OrderDetails>()
                .Property(e => e.s_Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_OrderDetails>()
                .Property(e => e.s_Discount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_OrderID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_TaskID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_BillID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_CustomerID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_Approver)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_Picker)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_Shipper)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_Recipient)
                .IsUnicode(false);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_ProPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_Taxes)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_Freight)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_Insurance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.s_TotalMoney)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Orders>()
                .Property(e => e.Timestamp)
                .IsFixedLength();

            modelBuilder.Entity<t_OrderTicket>()
                .Property(e => e.s_OrderID)
                .IsUnicode(false);

            modelBuilder.Entity<t_OrderTicket>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_OrderTracker>()
                .Property(e => e.s_OrderID)
                .IsUnicode(false);

            modelBuilder.Entity<t_OrderTracker>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_OrderTracker>()
                .Property(e => e.s_ReferTo)
                .IsUnicode(false);

            modelBuilder.Entity<t_Products>()
                .Property(e => e.s_Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_Products>()
                .Property(e => e.s_Discount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_RolePermission>()
                .Property(e => e.s_RolePermissionRule)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Attachments>()
                .Property(e => e.s_TaskID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Attachments>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Contacts>()
                .Property(e => e.s_TaskID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Contacts>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Department>()
                .Property(e => e.s_Task_DeptName_EN)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Group>()
                .Property(e => e.s_Task_GroupName_EN)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Group_User_Ref>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Quotes>()
                .Property(e => e.s_TaskID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Quotes>()
                .Property(e => e.s_Task_QuoteKey)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Step>()
                .Property(e => e.s_TaskID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Step>()
                .Property(e => e.s_AddUser)
                .IsUnicode(false);

            modelBuilder.Entity<t_Task_Step>()
                .Property(e => e.s_ReferTo)
                .IsUnicode(false);

            modelBuilder.Entity<t_Tasks>()
                .Property(e => e.s_TaskID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Tasks>()
                .Property(e => e.s_RelationEntity)
                .IsUnicode(false);

            modelBuilder.Entity<t_Tasks>()
                .Property(e => e.s_RelationEntityID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Tasks>()
                .Property(e => e.s_CustomerID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Tasks>()
                .Property(e => e.s_Sponsor)
                .IsUnicode(false);

            modelBuilder.Entity<t_Tasks>()
                .Property(e => e.s_Operator)
                .IsUnicode(false);

            modelBuilder.Entity<t_TransactionRecord>()
                .Property(e => e.s_TransactionID)
                .IsUnicode(false);

            modelBuilder.Entity<t_TransactionRecord>()
                .Property(e => e.s_CustomerID)
                .IsUnicode(false);

            modelBuilder.Entity<t_TransactionRecord>()
                .Property(e => e.s_OrderID)
                .IsUnicode(false);

            modelBuilder.Entity<t_TransactionRecord>()
                .Property(e => e.s_Debit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_TransactionRecord>()
                .Property(e => e.s_Credit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_TransactionRecord>()
                .Property(e => e.s_CurrentBalance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_TransactionRecord>()
                .Property(e => e.s_TransAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<t_User_Group_Ref>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_User_Role_Ref>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_UserGroup>()
                .Property(e => e.s_GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<t_UserRole>()
                .Property(e => e.s_RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<t_Users>()
                .Property(e => e.s_UserID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Users>()
                .Property(e => e.s_UserPwd)
                .IsUnicode(false);

            modelBuilder.Entity<t_Users>()
                .Property(e => e.s_Sex)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
