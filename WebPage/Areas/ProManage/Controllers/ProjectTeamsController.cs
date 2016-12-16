using Common;
using Domain;
using Microsoft.CSharp.RuntimeBinder;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.ProManage.Controllers
{
    public class ProjectTeamsController : BaseController
    {

        private IProjectTeamManage ProjectTeamManage
        {
            get;
            set;
        }

        private IProjectStageManage ProjectStageManage
        {
            get;
            set;
        }

        private IProjectMessage ProjectMessage
        {
            get;
            set;
        }

        [UserAuthorize(ModuleAlias = "ProjectTeams", OperaAction = "View")]
        public ActionResult Index(int id)
        {
            ActionResult result;
            try
            {
                List<int> stagelist = (from p in this.ProjectStageManage.LoadAll((PRO_PROJECT_STAGES p) => p.FK_ProjectId == id)
                                       select p.ID).ToList<int>();
                result = base.View(this.BindList(stagelist));
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "团队管理加载主页：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectTeams", OperaAction = "Audit")]
        public ActionResult Audit()
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "审核成功",
                Status = "n"
            };
            string s = base.Request.Form["idList"];
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    int teamId = int.Parse(s);
                    PRO_PROJECT_TEAMS Team = this.ProjectTeamManage.Get((PRO_PROJECT_TEAMS p) => p.ID == teamId);
                    Team.JionStatus = ClsDic.DicStatus["通过"];
                    Team.JionDate = DateTime.Now;
                    this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                    {
                        FK_ProjectId = Team.PRO_PROJECT_STAGES.FK_ProjectId,
                        CreateDate = DateTime.Now,
                        UserName = base.CurrentUser.Name,
                        UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                        MessageContent = "允许" + this.UserManage.Get((SYS_USER m) => m.ID == Team.FK_UserId).NAME + "加入了团队"
                    });
                    this.ProjectTeamManage.Update(Team);
                    jsonHelper.Status = "y";
                    base.WriteLog(enumOperator.Audit, "允许用户加入团队：" + jsonHelper.Msg, enumLog4net.INFO);
                    transactionScope.Complete();
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "审核失败，请稍后再试！";
                    base.WriteLog(enumOperator.Audit, "审核加入团队：", e);
                }
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "ProjectTeams", OperaAction = "Refuse")]
        public ActionResult Refuse()
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "驳回成功",
                Status = "n"
            };
            string s = base.Request.Form["idList"];
            string refuseReasons = base.Request.Form["reason"];
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    int teamId = int.Parse(s);
                    PRO_PROJECT_TEAMS Team = this.ProjectTeamManage.Get((PRO_PROJECT_TEAMS p) => p.ID == teamId);
                    Team.JionStatus = ClsDic.DicStatus["驳回"];
                    Team.JionDate = DateTime.Now;
                    Team.RefuseReasons = refuseReasons;
                    this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                    {
                        FK_ProjectId = Team.PRO_PROJECT_STAGES.FK_ProjectId,
                        CreateDate = DateTime.Now,
                        UserName = base.CurrentUser.Name,
                        UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                        MessageContent = "拒绝" + this.UserManage.Get((SYS_USER m) => m.ID == Team.FK_UserId).NAME + "加入团队"
                    });
                    this.ProjectTeamManage.Update(Team);
                    jsonHelper.Status = "y";
                    base.WriteLog(enumOperator.Audit, "拒绝用户加入团队：" + jsonHelper.Msg, enumLog4net.INFO);
                    transactionScope.Complete();
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "审核失败，请稍后再试！";
                    base.WriteLog(enumOperator.Audit, "审核加入团队：", e);
                }
            }
            return base.Json(jsonHelper);
        }

        private PageInfo BindList(List<int> stagelist)
        {
            IQueryable<PRO_PROJECT_TEAMS> queryable = this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => stagelist.Any((int e) => e == p.FK_StageId));
            queryable = from p in queryable
                        orderby p.JionStatus descending
                        orderby p.JionDate
                        select p;
            PageInfo<PRO_PROJECT_TEAMS> pageInfo = this.ProjectTeamManage.Query(queryable, base.page, base.pagesize);
            var obj = pageInfo.List.Select(delegate (PRO_PROJECT_TEAMS p)
            {
                return new
                {
                    ID = p.ID,
                    UserFace = (this.UserManage.Get((SYS_USER m) => m.ID == p.FK_UserId) == null) ? "" : (string.IsNullOrEmpty(this.UserManage.Get((SYS_USER m) => m.ID == p.FK_UserId).FACE_IMG) ? ("<img alt=\"image\" class=\"img-circle\" src=\"/Pro/Project/User_Default_Avatat?name=" + this.UserManage.Get((SYS_USER m) => m.ID == p.FK_UserId).NAME.Substring(0, 1).ToUpper() + "\" />") : ("<img alt=\"image\" class=\"img-circle\" src=\"" + this.UserManage.Get((SYS_USER m) => m.ID == p.FK_UserId).FACE_IMG + "\" />")),
                    UserName = (this.UserManage.Get((SYS_USER m) => m.ID == p.FK_UserId) == null) ? "" : this.UserManage.Get((SYS_USER m) => m.ID == p.FK_UserId).NAME,
                    Status = Tools.GetEnumText<enumStatusType>(p.JionStatus),
                    JionStatus = p.JionStatus,
                    JionDate = p.JionDate,
                    ApplyReasons = p.ApplyReasons,
                    SurplusNumber = this.ProjectStageManage.Get((PRO_PROJECT_STAGES m) => m.ID == p.FK_StageId).NeedNumber - (from n in this.ProjectStageManage.Get((PRO_PROJECT_STAGES m) => m.ID == p.FK_StageId).PRO_PROJECT_TEAMS
                                                                                                                              where n.JionStatus == ClsDic.DicStatus["通过"]
                                                                                                                              select n).ToList<PRO_PROJECT_TEAMS>().Count,
                    StageTitle = this.ProjectStageManage.Get((PRO_PROJECT_STAGES m) => m.ID == p.FK_StageId).StageTitle
                };
            }).ToList();
            //if (ProjectTeamsController.< BindList > o__SiteContainera.<> p__Siteb == null)
            //{
            //    ProjectTeamsController.< BindList > o__SiteContainera.<> p__Siteb = CallSite<Func<CallSite, Type, int, int, int, object, PageInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(ProjectTeamsController), new CSharpArgumentInfo[]
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
