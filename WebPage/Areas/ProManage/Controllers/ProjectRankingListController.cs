using Common;
using Domain;
using Microsoft.CSharp.RuntimeBinder;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.ProManage.Controllers
{
    public class ProjectRankingListController : BaseController
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

        private IProjectFilesManage ProjectFilesManage
        {
            get;
            set;
        }

        [UserAuthorize(ModuleAlias = "RankingList", OperaAction = "View")]
        public ActionResult Index()
        {
            ActionResult result;
            try
            {
                string value = base.Request.QueryString["rank"];
                IQueryable<SYS_USER> source = (string.IsNullOrEmpty(value) && !base.CurrentUser.IsAdmin) ? this.UserManage.LoadAll((SYS_USER p) => p.DPTID == this.CurrentUser.DptInfo.ID) : this.UserManage.LoadAll(null);
                IQueryable<PRO_PROJECT_STAGES> source2 = (string.IsNullOrEmpty(value) && !base.CurrentUser.IsAdmin) ? this.ProjectStageManage.LoadAll((PRO_PROJECT_STAGES p) => p.PRO_PROJECTS.Fk_DepartId == this.CurrentUser.DptInfo.ID) : this.ProjectStageManage.LoadAll(null);
                List<PRO_PROJECT_FILES> source3 = this.ProjectFilesManage.LoadListAll((PRO_PROJECT_FILES p) => p.DocStyle == "projectstage");
                int JoinStatus = ClsDic.DicStatus["通过"];
                List<UserStageRank> list = new List<UserStageRank>();
                int count = source2.ToList<PRO_PROJECT_STAGES>().Count;
                using (List<SYS_USER>.Enumerator enumerator = source.ToList<SYS_USER>().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        SYS_USER user = enumerator.Current;
                        int count2 = this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == user.ID && p.JionStatus == JoinStatus).ToList<PRO_PROJECT_TEAMS>().Count;
                        IEnumerable<PRO_PROJECT_FILES> source4 = from p in source3
                                                                 where p.FK_UserId == user.ID
                                                                 select p;
                        int count3 = source4.ToList<PRO_PROJECT_FILES>().Count;
                        List<UserStageRank> arg_53D_0 = list;
                        UserStageRank userStageRank = new UserStageRank();
                        userStageRank.UserId = user.ID;
                        userStageRank.UserZhiWu = user.LEVELS;
                        userStageRank.UserFace = (string.IsNullOrEmpty(user.FACE_IMG) ? ("/Pro/Project/User_Default_Avatat?name=" + user.NAME.Substring(0, 1)) : user.FACE_IMG);
                        userStageRank.UserName = user.NAME;
                        userStageRank.UserAccount = user.ACCOUNT;
                        userStageRank.EntyTime = ((user.SYS_USERINFO != null && user.SYS_USERINFO.Count > 0) ? string.Format("{0:yyyy-MM-dd}", user.SYS_USERINFO.FirstOrDefault<SYS_USERINFO>().JINRUDATE) : "");
                        userStageRank.PartakeCount = count2;
                        userStageRank.PartakePercent = ((count > 0) ? ((double)count2 / ((double)count * 1.0) * 100.0) : 0.0);
                        UserStageRank arg_536_0 = userStageRank;
                        int arg_536_1;
                        if (count3 <= 0)
                        {
                            arg_536_1 = 0;
                        }
                        else
                        {
                            arg_536_1 = (int)Math.Floor((double)(from p in source4
                                                                 where p.AcceptanceStatus == ClsDic.DicStatus["通过"]
                                                                 select p).ToList<PRO_PROJECT_FILES>().Count / ((double)count3 * 1.0) * 100.0);
                        }
                        arg_536_0.AdoptPercent = arg_536_1;
                        arg_53D_0.Add(userStageRank);
                    }
                }
                List<UserStageRank> list2 = (from p in list
                                             orderby p.PartakeCount descending
                                             orderby p.AdoptPercent descending
                                             select p).Take(11).ToList<UserStageRank>();
                UserStageRank arg_8F0_0;
                if ((from p in list
                     where p.UserId == base.CurrentUser.Id
                     select p).ToList<UserStageRank>().FirstOrDefault<UserStageRank>() != null)
                {
                    arg_8F0_0 = (from p in list
                                 where p.UserId == base.CurrentUser.Id
                                 select p).ToList<UserStageRank>().FirstOrDefault<UserStageRank>();
                }
                else
                {
                    UserStageRank userStageRank2 = new UserStageRank();
                    userStageRank2.UserId = base.CurrentUser.Id;
                    userStageRank2.UserZhiWu = base.CurrentUser.Levels;
                    userStageRank2.UserFace = (string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1)) : base.CurrentUser.Face_Img);
                    userStageRank2.UserName = base.CurrentUser.Name;
                    userStageRank2.PartakeCount = this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == this.CurrentUser.Id && p.JionStatus == JoinStatus).ToList<PRO_PROJECT_TEAMS>().Count;
                    userStageRank2.PartakePercent = ((count > 0) ? ((double)this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == this.CurrentUser.Id && p.JionStatus == JoinStatus).ToList<PRO_PROJECT_TEAMS>().Count / ((double)count * 1.0) * 100.0) : 0.0);
                    UserStageRank arg_8E9_0 = userStageRank2;
                    int arg_8E9_1;
                    if ((from p in source3
                         where p.FK_UserId == base.CurrentUser.Id
                         select p).ToList<PRO_PROJECT_FILES>().Count <= 0)
                    {
                        arg_8E9_1 = 0;
                    }
                    else
                    {
                        arg_8E9_1 = (int)Math.Floor((double)(from p in source3
                                                             where p.FK_UserId == base.CurrentUser.Id && p.AcceptanceStatus == ClsDic.DicStatus["通过"]
                                                             select p).ToList<PRO_PROJECT_FILES>().Count / ((double)(from p in source3
                                                                                                                     where p.FK_UserId == base.CurrentUser.Id
                                                                                                                     select p).ToList<PRO_PROJECT_FILES>().Count * 1.0) * 100.0);
                    }
                    arg_8E9_0.AdoptPercent = arg_8E9_1;
                    arg_8F0_0 = userStageRank2;
                }
                UserStageRank userStageRank3 = arg_8F0_0;
                base.ViewData["CurrentRank"] = userStageRank3;
                ViewDataDictionary arg_981_0 = base.ViewData;
                string arg_981_1 = "CurrentRankNumber";
                int arg_97C_0;
                if (list2.IndexOf(userStageRank3) < 0)
                {
                    arg_97C_0 = (from p in list
                                 orderby p.PartakeCount descending
                                 orderby p.AdoptPercent descending
                                 select p).ToList<UserStageRank>().IndexOf(userStageRank3) + 1;
                }
                else
                {
                    arg_97C_0 = list2.IndexOf(userStageRank3) + 1;
                }
                arg_981_0[arg_981_1] = arg_97C_0;
                base.ViewData["StageCount"] = count;
                base.ViewData["RankStyle"] = (string.IsNullOrEmpty(value) ? "depart" : "all");
                base.ViewData["EmailDomain"] = base.EmailDomain;
                //if (ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site3 == null)
                //{
                //    ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site3 = CallSite<Func<CallSite, object, ActionResult>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(ActionResult), typeof(ProjectRankingListController)));
                //}
                //Func<CallSite, object, ActionResult> arg_A74_0 = ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site3.Target;
                //CallSite arg_A74_1 = ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site3;
                //if (ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site4 == null)
                //{
                //    ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site4 = CallSite<Func<CallSite, ProjectRankingListController, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "View", null, typeof(ProjectRankingListController), new CSharpArgumentInfo[]
                //    {
                //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
                //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                //    }));
                //}
                result = null;
                //result = arg_A74_0(arg_A74_1, ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site4.Target(ProjectRankingListController.< Index > o__SiteContainer2.<> p__Site4, this, JsonConverter.JsonClass(list2)));
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "加载排行榜：", ex);
                throw ex.InnerException;
            }
            return result;
        }
    }
}
