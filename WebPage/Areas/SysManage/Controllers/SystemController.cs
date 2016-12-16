using Common;
using Domain;
using Microsoft.CSharp.RuntimeBinder;
using Service.IService;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.SysManage.Controllers
{
    public class SystemController : BaseController
    {


        private ISystemManage SystemManage
        {
            get;
            set;
        }

        private IModuleManage ModuleManage
        {
            get;
            set;
        }

        [UserAuthorize(ModuleAlias = "SystemSet", OperaAction = "View")]
        public ActionResult Index()
        {
            string text = base.Request.QueryString["status"];
            base.ViewData["status"] = text;
            //if (SystemController.< Index > o__SiteContainer0.<> p__Site1 == null)
            //{
            //    SystemController.< Index > o__SiteContainer0.<> p__Site1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Search", typeof(SystemController), new CSharpArgumentInfo[]
            //    {
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
            //    }));
            //}
            //SystemController.< Index > o__SiteContainer0.<> p__Site1.Target(SystemController.< Index > o__SiteContainer0.<> p__Site1, base.ViewBag, base.keywords);
            return base.View(this.BindList(text));
        }

        [UserAuthorize(ModuleAlias = "SystemSet", OperaAction = "Detail")]
        public ActionResult Detail(string id)
        {
            return base.View(this.SystemManage.Get((SYS_SYSTEM p) => p.ID == id) ?? new SYS_SYSTEM());
        }

        [ValidateInput(false), UserAuthorize(ModuleAlias = "SystemSet", OperaAction = "Add,Edit")]
        public ActionResult Save(SYS_SYSTEM entity)
        {
            bool flag = false;
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "保存系统成功",
                Status = "n"
            };
            try
            {
                if (entity != null)
                {
                    if (string.IsNullOrEmpty(entity.ID))
                    {
                        entity.ID = Guid.NewGuid().ToString();
                        entity.CREATEDATE = new DateTime?(DateTime.Now);
                    }
                    else
                    {
                        flag = true;
                    }
                    if (!this.SystemManage.IsExist((SYS_SYSTEM p) => p.ID != entity.ID && p.NAME == entity.NAME && p.SITEURL == entity.SITEURL))
                    {
                        if (this.SystemManage.SaveOrUpdate(entity, flag))
                        {
                            jsonHelper.Status = "y";
                        }
                        else
                        {
                            jsonHelper.Msg = "保存系统失败";
                        }
                    }
                    else
                    {
                        jsonHelper.Msg = entity.NAME + "系统已存在，不能重复添加";
                    }
                }
                else
                {
                    jsonHelper.Msg = "未找到需要保存的系统";
                }
                if (flag)
                {
                    base.WriteLog(enumOperator.Edit, "修改系统，结果：" + jsonHelper.Msg, enumLog4net.INFO);
                }
                else
                {
                    base.WriteLog(enumOperator.Add, "添加系统，结果：" + jsonHelper.Msg, enumLog4net.INFO);
                }
            }
            catch (Exception e)
            {
                jsonHelper.Msg = "保存系统发生内部错误！";
                base.WriteLog(enumOperator.None, "保存系统：", e);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "SystemSet", OperaAction = "Remove")]
        public ActionResult Delete(string idlist)
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "删除成功",
                Status = "n"
            };
            try
            {
                idlist = idlist.TrimEnd(new char[]
                {
                    ','
                });
                if (!string.IsNullOrEmpty(idlist))
                {
                    if (idlist.ToLower().Contains("fddeab19-3588-4fe1-83b6-c15d4abb942d"))
                    {
                        jsonHelper.Msg = "不能删除主系统";
                    }
                    else if (this.SystemManage.IsExist((SYS_SYSTEM p) => idlist.Contains(p.ID) && p.IS_LOGIN))
                    {
                        jsonHelper.Msg = "要删除的系统正在使用中，不能删除";
                    }
                    else if (this.ModuleManage.IsExist((SYS_MODULE p) => idlist.Contains(p.FK_BELONGSYSTEM)))
                    {
                        jsonHelper.Msg = "要删除的系统存在使用中的模块，不能删除";
                    }
                    else
                    {
                        this.SystemManage.Delete((SYS_SYSTEM p) => idlist.Contains(p.ID));
                        jsonHelper.Status = "y";
                    }
                }
                else
                {
                    jsonHelper.Msg = "未找到要删除的系统记录";
                }
                base.WriteLog(enumOperator.Remove, "删除系统：" + jsonHelper.Msg, enumLog4net.WARN);
            }
            catch (Exception e)
            {
                jsonHelper.Msg = "删除系统发生内部错误！";
                base.WriteLog(enumOperator.Remove, "删除系统：", e);
            }
            return base.Json(jsonHelper);
        }

        private PageInfo BindList(string status)
        {
            IQueryable<SYS_SYSTEM> queryable = this.SystemManage.LoadAll((SYS_SYSTEM p) => this.CurrentUser.System_Id.Any((string e) => e == p.ID));
            if (!string.IsNullOrEmpty(status))
            {
                bool islogin = status == "True";
                queryable = from p in queryable
                            where p.IS_LOGIN == islogin
                            select p;
            }
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                queryable = from p in queryable
                            where p.NAME.Contains(this.keywords)
                            select p;
            }
            queryable = from p in queryable
                        orderby p.CREATEDATE descending
                        select p;
            PageInfo<SYS_SYSTEM> pageInfo = this.SystemManage.Query(queryable, base.page, base.pagesize);
            var obj = pageInfo.List.Select((SYS_SYSTEM p, int i) => new
            {
                ID = p.ID,
                NAME = p.NAME,
                SITEURL = p.SITEURL,
                ISLOGIN = p.IS_LOGIN ? "<i class=\"fa fa-circle text-navy\"></i>" : "<i class=\"fa fa-circle text-danger\"></i>",
                CREATEDATE = p.CREATEDATE
            });
            //if (SystemController.< BindList > o__SiteContainera.<> p__Siteb == null)
            //{
            //    SystemController.< BindList > o__SiteContainera.<> p__Siteb = CallSite<Func<CallSite, Type, int, int, int, object, PageInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(SystemController), new CSharpArgumentInfo[]
            //    {
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
            //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            //    }));
            //}
            return new PageInfo( pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(obj));
        }
    }
}
