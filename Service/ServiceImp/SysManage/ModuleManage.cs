using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Service.ServiceImp
{
    /// <summary>
    /// Service模型处理类
    ///  2015-05-22
    /// </summary>
    public class ModuleManage : RepositoryBase<Domain.SYS_MODULE>, IService.IModuleManage
    {

        private IPermissionManage PermissionManage
        {
            get;
            set;
        }

        private IUserPermissionManage UserPermissionManage
        {
            get;
            set;
        }

        private IRolePermissionManage RolePermissionManage
        {
            get;
            set;
        }

        /// <summary>
        /// 获取用户权限模块集合
        ///  2015-05-30
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permission">用户授权集合</param>
        /// <param name="siteId">站点ID</param>
        /// <returns></returns>
        public List<Domain.SYS_MODULE> GetModule(int userId, List<Domain.SYS_PERMISSION> permission, List<string> systemid)
        {
            //返回模块
            var retmodule = new List<Domain.SYS_MODULE>();
            var permodule = new List<Domain.SYS_MODULE>();
            //权限转模块
            if (permission != null)
            {
                permodule.AddRange(permission.Select(p => p.SYS_MODULE));
                //去重
                permodule = permodule.Distinct(new ModuleDistinct()).ToList();
            }
            //检索显示与系统
            permodule = permodule.Where(p => p.ISSHOW && systemid.Any(e => e == p.FK_BELONGSYSTEM)).ToList();
            //构造上级导航模块
            var prevModule = this.LoadListAll(p => systemid.Any(e => e == p.FK_BELONGSYSTEM));
            //反向递归算法构造模块带上级上上级模块
            if (permodule.Count > 0)
            {
                foreach (var item in permodule)
                {
                    RecursiveModule(prevModule, retmodule, item.PARENTID);
                    retmodule.Add(item); 
                }
            }
            //去重
            retmodule = retmodule.Distinct(new ModuleDistinct()).ToList();
            //返回模块集合
            return retmodule.OrderBy(p => p.LEVELS).ThenBy(p => p.SHOWORDER).ToList();
        }

        /// <summary>
        /// 反向递归模块集合，可重复模块数据，最后去重
        /// </summary>
        /// <param name="PrevModule">总模块</param>
        /// <param name="retmodule">返回模块</param>
        /// <param name="parentId">上级ID</param>
        private void RecursiveModule(List<Domain.SYS_MODULE> PrevModule, List<Domain.SYS_MODULE> retmodule, int? parentId)
        {
            var result = PrevModule.Where(p => p.ID == parentId);
            if (result != null)
            {
                foreach (var item in result)
                {
                    retmodule.Add(item);
                    RecursiveModule(PrevModule, retmodule, item.PARENTID);
                }
            }
        }

        /// <summary>
        /// 递归模块列表，返回按级别排序
        ///  2015-06-03
        /// </summary>
        public List<Domain.SYS_MODULE> RecursiveModule(List<Domain.SYS_MODULE> list)
        {
            List<Domain.SYS_MODULE> result = new List<Domain.SYS_MODULE>();
            if (list != null && list.Count > 0)
            {
                ChildModule(list, result, 0);
            }
            return result;
        }
        /// <summary>
        /// 递归模块列表
        ///  2015-06-03
        /// </summary>
        private void ChildModule(List<Domain.SYS_MODULE> list, List<Domain.SYS_MODULE> newlist, int parentId)
        {
            var result = list.Where(p => p.PARENTID == parentId).OrderBy(p => p.LEVELS).OrderBy(p => p.SHOWORDER).ToList();
            if (result.Count() > 0)
            {
                for (int i = 0; i < result.Count(); i++)
                {
                    newlist.Add(result[i]);
                    ChildModule(list, newlist, result[i].ID);
                }
            }
        }

        public List<SYS_MODULE> RecursiveChildModule(List<SYS_MODULE> list)
        {
            List<SYS_MODULE> result;
            try
            {
                List<int> listId = (from p in list
                                    select p.ID).ToList<int>();
                List<SYS_MODULE> list2 = this.LoadAll((SYS_MODULE p) => listId.Contains(p.PARENTID)).ToList<SYS_MODULE>();
                if (list2.Count > 0)
                {
                    list.AddRange(this.RecursiveChildModule(list2));
                }
                result = list;
            }
            catch
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 批量变更下级模块的级别
        /// </summary>
        public bool MoreModifyModule(int moduleId, int levels)
        {
            //根据当前模块ID获取下级模块的集合
            var ChildModule = this.LoadAll(p => p.PARENTID == moduleId).ToList();
            if (ChildModule.Any())
            {
                foreach (var item in ChildModule)
                {
                    item.LEVELS = levels + 1;
                    this.Update(item);
                    MoreModifyModule(item.ID, item.LEVELS);
                }
            }
            return true;
        }

        public bool Remove(List<string> moduleId)
        {
            bool result = false;
            try
            {
                List<SYS_MODULE> list = this.LoadAll((SYS_MODULE p) => moduleId.Contains(p.ID.ToString())).ToList<SYS_MODULE>();
                List<int> moduleIDList = (from p in this.RecursiveChildModule(list)
                                          select p.ID).ToList<int>();
                List<int> permissionIDList = (from p in this.PermissionManage.LoadAll((SYS_PERMISSION p) => moduleIDList.Contains(p.MODULEID))
                                              select p.ID).ToList<int>();
                List<int> rolePerIdList = (from p in this.RolePermissionManage.LoadAll((SYS_ROLE_PERMISSION p) => permissionIDList.Contains(p.PERMISSIONID))
                                           select p.ID).ToList<int>();
                List<int> userPerIdList = (from p in this.UserPermissionManage.LoadAll((SYS_USER_PERMISSION p) => permissionIDList.Contains(p.FK_PERMISSIONID))
                                           select p.ID).ToList<int>();
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    this.RolePermissionManage.Delete((SYS_ROLE_PERMISSION p) => rolePerIdList.Contains(p.ID));
                    this.UserPermissionManage.Delete((SYS_USER_PERMISSION p) => userPerIdList.Contains(p.ID));
                    this.PermissionManage.Delete((SYS_PERMISSION p) => permissionIDList.Contains(p.ID));
                    this.Delete((SYS_MODULE p) => moduleIDList.Contains(p.ID));
                    result = true;
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// 获取模板列表
        /// </summary>
        public dynamic LoadModuleInfo(int id)
        {
            return Common.JsonConverter.JsonClass(this.LoadAll(p => p.PARENTID == id).OrderBy(p => p.ID).Select(p => new { p.ID, p.NAME }).ToList());
        }
    }
    /// <summary>
    /// 模型去重，非常重要
    ///  2015-08-03
    /// </summary>
    public class ModuleDistinct : IEqualityComparer<Domain.SYS_MODULE>
    {
        public bool Equals(Domain.SYS_MODULE x, Domain.SYS_MODULE y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Domain.SYS_MODULE obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}