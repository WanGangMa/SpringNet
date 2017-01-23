namespace Service.IService
{
    /// <summary>
    /// Service层用户授权接口
    ///  2016-05-19
    /// </summary>
    public interface IUserPermissionManage : IRepository<Domain.SYS_USER_PERMISSION>, IAutofac
    {
        /// <summary>
        /// 设置用户权限
        ///  2016-05-19
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newper">权限字符串</param>
        /// <param name="sysId">系统ID</param>
        /// <returns></returns>
        bool SetUserPermission(int userId, string newper); //, string sysId);
    }
}