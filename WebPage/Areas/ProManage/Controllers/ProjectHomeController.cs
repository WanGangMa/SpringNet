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
    public class ProjectHomeController : BaseController
    {

        private IProjectManage ProjectManage
        {
            get;
            set;
        }

        private IBussinessCustomerManage BussinessCustomerManage
        {
            get;
            set;
        }

        private ICodeAreaManage CodeAreaManage
        {
            get;
            set;
        }

        private IContentManage ContentManage
        {
            get;
            set;
        }

        private ICodeManage CodeManage
        {
            get;
            set;
        }

        private IProjectFilesManage ProjectFilesManage
        {
            get;
            set;
        }

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

        [UserAuthorize(ModuleAlias = "ProjectHome", OperaAction = "View")]
        public ActionResult Index()
        {
            ActionResult result;
            try
            {
                string text = base.Request.QueryString["ProjectType"];
                base.ViewData["ProjectType"] = text;
               
                //ProjectHomeController.< Index > o__SiteContainer0.<> p__Site1.Target(ProjectHomeController.< Index > o__SiteContainer0.<> p__Site1, base.ViewBag, base.keywords);
                base.ViewData["ProjectTypeList"] = Tools.BindEnumsList(typeof(enumProjectType));
                result = base.View(this.BindList(text));
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "项目中心加载主页：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectHome", OperaAction = "View")]
        public ActionResult Detail(int id)
        {
            ActionResult result;
            try
            {
                PRO_PROJECTS entity = this.ProjectManage.Get((PRO_PROJECTS p) => p.ID == id);
                base.ViewData["ProjectDescribe"] = ((this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "PRO_PROJECTS") == null) ? "" : this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "PRO_PROJECTS").CONTENT);
                base.ViewData["ProjectDocument"] = JsonConverter.JsonClass((from p in this.ProjectFilesManage.LoadListAll((PRO_PROJECT_FILES p) => p.DocStyle == "project" && p.Fk_ForeignId == entity.ID)
                                                                            select new
                                                                            {
                                                                                DocName = p.DocName,
                                                                                DocPath = p.DocPath,
                                                                                Icon = FileHelper.GetFileIcon(p.DocFileExt)
                                                                            }).ToList());
                base.ViewData["BussinessCustomer"] = ((this.BussinessCustomerManage.Get((SYS_BUSSINESSCUSTOMER m) => m.ID == entity.Fk_BussinessCustomer) == null) ? "" : this.BussinessCustomerManage.Get((SYS_BUSSINESSCUSTOMER m) => m.ID == entity.Fk_BussinessCustomer).CompanyName);
                base.ViewData["ProjectManager"] = ((this.UserManage.Get((SYS_USER m) => m.ID == entity.Fk_UserId) == null) ? "" : this.UserManage.Get((SYS_USER m) => m.ID == entity.Fk_UserId).NAME);
                base.ViewData["ProjectTeams"] = this.GetTeams(entity.PRO_PROJECT_STAGES.ToList<PRO_PROJECT_STAGES>());
                base.ViewData["ProjectProgress"] = this.GetProgress(entity.PRO_PROJECT_STAGES.ToList<PRO_PROJECT_STAGES>());
                result = base.View(entity);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "项目中心加载详情：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectHome", OperaAction = "JoinTeam")]
        public ActionResult JoinTeam(int id)
        {
            ActionResult result;
            try
            {
                PRO_PROJECTS model = this.ProjectManage.Get((PRO_PROJECTS p) => p.ID == id);
                result = base.View(model);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "项目中心加入团队：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectHome", OperaAction = "Acceptance")]
        public ActionResult Acceptance(int id)
        {
            ActionResult result;
            try
            {
                PRO_PROJECTS pRO_PROJECTS = this.ProjectManage.Get((PRO_PROJECTS p) => p.ID == id);
                base.ViewData["ProjectProgress"] = this.GetProgress(pRO_PROJECTS.PRO_PROJECT_STAGES.ToList<PRO_PROJECT_STAGES>());
              
                //ProjectHomeController.< Acceptance > o__SiteContainera.<> p__Siteb.Target(ProjectHomeController.< Acceptance > o__SiteContainera.<> p__Siteb, base.ViewBag, base.CurrentUser.Id);
                result = base.View(pRO_PROJECTS);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "项目中心阶段验收加载阶段任务：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        private PageInfo BindList(string ProjectType)
        {
            IQueryable<PRO_PROJECTS> queryable = this.ProjectManage.LoadAll(null);
            if (!base.CurrentUser.IsAdmin)
            {
                queryable = from p in queryable
                            where p.Fk_DepartId == this.CurrentUser.DptInfo.ID
                            select p;
            }
            if (!string.IsNullOrEmpty(ProjectType))
            {
                int projectStatus = int.Parse(ProjectType);
                queryable = from p in queryable
                            where p.ProjectStatus == projectStatus
                            select p;
            }
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                queryable = from p in queryable
                            where p.ProjectTitle.Contains(this.keywords)
                            select p;
            }
            queryable = from p in queryable
                        orderby p.ProjectStatus
                        orderby p.ProjectLevels, p.EndDate, p.CreateDate descending
                        select p;
            PageInfo<PRO_PROJECTS> pageInfo = this.ProjectManage.Query(queryable, base.page, base.pagesize);
            var obj = (from p in pageInfo.List
                       select new
                       {
                           ID = p.ID,
                           ProjectTitle = p.ProjectTitle,
                           Status = p.ProjectStatus,
                           ProjectStatus = Tools.GetEnumText<enumProjectType>(p.ProjectStatus),
                           ProjectLevels = Tools.GetEnumText<enumProjectLevels>(p.ProjectLevels),
                           CreateDate = p.CreateDate.ToString("yyyy.MM.dd"),
                           Teams = this.GetTeams(p.PRO_PROJECT_STAGES.ToList<PRO_PROJECT_STAGES>()),
                           Progerss = this.GetProgress(p.PRO_PROJECT_STAGES.ToList<PRO_PROJECT_STAGES>())
                       }).ToList();
           
            return new PageInfo( pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(obj));
        }

        private string GetTeams(List<PRO_PROJECT_STAGES> Stages)
        {
            string text = string.Empty;
            List<int> list = new List<int>();
            if (Stages != null && Stages.Count > 0)
            {
                foreach (PRO_PROJECT_STAGES current in Stages)
                {
                    if (current.PRO_PROJECT_TEAMS != null && current.PRO_PROJECT_TEAMS.Count > 0)
                    {
                        using (IEnumerator<PRO_PROJECT_TEAMS> enumerator2 = current.PRO_PROJECT_TEAMS.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                PRO_PROJECT_TEAMS team = enumerator2.Current;
                                if (team.JionStatus == ClsDic.DicStatus["通过"] && !list.Contains(team.FK_UserId))
                                {
                                    list.Add(team.FK_UserId);
                                    text = text + "&nbsp;" + ((this.UserManage.Get((SYS_USER p) => p.ID == team.FK_UserId) == null) ? "" : (string.IsNullOrEmpty(this.UserManage.Get((SYS_USER p) => p.ID == team.FK_UserId).FACE_IMG) ? string.Concat(new string[]
                                    {
                                        "<img alt=\"image\" class=\"img-circle\" src=\"/Pro/Project/User_Default_Avatat?name=",
                                        this.UserManage.Get((SYS_USER p) => p.ID == team.FK_UserId).NAME.Substring(0, 1).ToUpper(),
                                        "\" data-toggle=\"tooltip\" data-placement=\"top\"  data-original-title=\"",
                                        this.UserManage.Get((SYS_USER p) => p.ID == team.FK_UserId).NAME,
                                        "\" />"
                                    }) : string.Concat(new string[]
                                    {
                                        "<img alt=\"image\" class=\"img-circle\" src=\"",
                                        this.UserManage.Get((SYS_USER p) => p.ID == team.FK_UserId).FACE_IMG,
                                        "\" data-toggle=\"tooltip\" data-placement=\"top\"  data-original-title=\"",
                                        this.UserManage.Get((SYS_USER p) => p.ID == team.FK_UserId).NAME,
                                        "\" />"
                                    })));
                                }
                            }
                        }
                    }
                }
            }
            return text;
        }

        public int GetProgress(List<PRO_PROJECT_STAGES> Stages)
        {
            int result = 0;
            if (Stages != null && Stages.Count > 0)
            {
                int count = Stages.Count;
                int count2 = (from p in Stages
                              where p.StageStatus == ClsDic.DicProject["已验收"] || p.StageStatus == ClsDic.DicProject["已超时"]
                              select p).ToList<PRO_PROJECT_STAGES>().Count;
                result = ((count > 0) ? ((int)Math.Floor((double)count2 / ((double)count * 1.0) * 100.0)) : 0);
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectHome", OperaAction = "JoinTeam")]
        public ActionResult ApplyJoinTeam()
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "申请成功，请等待审核",
                Status = "n"
            };
            string text = base.Request.Form["FK_StageId"];
            string text2 = base.Request.Form["ApplyReasons"];
            if (string.IsNullOrEmpty(text))
            {
                jsonHelper.Msg = "请选择阶段任务";
                return base.Json(jsonHelper);
            }
            if (string.IsNullOrEmpty(text2))
            {
                jsonHelper.Msg = "请填写申请理由";
                return base.Json(jsonHelper);
            }
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    int StageId = int.Parse(text);
                    this.ProjectTeamManage.Save(new PRO_PROJECT_TEAMS
                    {
                        FK_StageId = StageId,
                        FK_UserId = base.CurrentUser.Id,
                        ApplyReasons = text2,
                        JionStatus = ClsDic.DicStatus["等待审核"],
                        JionDate = DateTime.Now
                    });
                    PRO_PROJECT_STAGES pRO_PROJECT_STAGES = this.ProjectStageManage.Get((PRO_PROJECT_STAGES p) => p.ID == StageId);
                    this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                    {
                        FK_ProjectId = pRO_PROJECT_STAGES.FK_ProjectId,
                        CreateDate = DateTime.Now,
                        UserName = base.CurrentUser.Name,
                        UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                        MessageContent = "申请加入团队，" + pRO_PROJECT_STAGES.StageTitle + "：" + text2
                    });
                    jsonHelper.Status = "y";
                    base.WriteLog(enumOperator.Allocation, "申请加入团队：" + jsonHelper.Msg, enumLog4net.INFO);
                    transactionScope.Complete();
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "申请失败，请稍后再试！";
                    base.WriteLog(enumOperator.Add, "申请加入团队：", e);
                }
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "ProjectHome", OperaAction = "Acceptance")]
        public ActionResult Audit()
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "验收成功",
                Status = "n"
            };
            string s = base.Request.Form["idList"];
            string s2 = base.Request.Form["docId"];
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    int stageId = int.Parse(s);
                    int adoptDocId = int.Parse(s2);
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("AcceptanceStatus", ClsDic.DicStatus["驳回"]);
                    this.ProjectFilesManage.Modify("PRO_PROJECT_FILES", dictionary, "and DocStyle='projectstage' and Fk_ForeignId = " + stageId);
                    PRO_PROJECT_FILES pRO_PROJECT_FILES = this.ProjectFilesManage.Get((PRO_PROJECT_FILES p) => p.ID == adoptDocId);
                    pRO_PROJECT_FILES.AcceptanceStatus = new int?(ClsDic.DicStatus["通过"]);
                    this.ProjectFilesManage.Update(pRO_PROJECT_FILES);
                    PRO_PROJECT_STAGES Stage = this.ProjectStageManage.Get((PRO_PROJECT_STAGES p) => p.ID == stageId);
                    if (Stage.EndDate < DateTime.Now)
                    {
                        Stage.StageStatus = ClsDic.DicProject["已超时"];
                        Stage.IsOverTime = true;
                        Stage.OverDays = (int)Math.Ceiling((DateTime.Now - Stage.EndDate).TotalHours);
                    }
                    else
                    {
                        Stage.StageStatus = ClsDic.DicProject["已验收"];
                    }
                    this.ProjectStageManage.Update(Stage);
                    PRO_PROJECTS pRO_PROJECTS = this.ProjectManage.Get((PRO_PROJECTS p) => p.ID == Stage.FK_ProjectId);
                    if ((from p in pRO_PROJECTS.PRO_PROJECT_STAGES
                         where p.StageStatus != ClsDic.DicProject["已超时"] && p.StageStatus != ClsDic.DicProject["已验收"]
                         select p).ToList<PRO_PROJECT_STAGES>().Count <= 0)
                    {
                        if (pRO_PROJECTS.EndDate < DateTime.Now)
                        {
                            pRO_PROJECTS.ProjectStatus = ClsDic.DicProject["已超时"];
                        }
                        else
                        {
                            pRO_PROJECTS.ProjectStatus = ClsDic.DicProject["已完成"];
                        }
                        this.ProjectManage.Update(pRO_PROJECTS);
                    }
                    this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                    {
                        FK_ProjectId = Stage.FK_ProjectId,
                        CreateDate = DateTime.Now,
                        UserName = base.CurrentUser.Name,
                        UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                        MessageContent = "验收了" + Stage.StageTitle + "阶段任务" + (Stage.IsOverTime ? (",已超时：" + Stage.OverDays + "天") : "")
                    });
                    jsonHelper.Status = "y";
                    base.WriteLog(enumOperator.Audit, "验收阶段任务：" + jsonHelper.Msg, enumLog4net.INFO);
                    transactionScope.Complete();
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "验收失败，请稍后再试！";
                    base.WriteLog(enumOperator.Audit, "验收阶段任务：", e);
                }
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "ProjectHome", OperaAction = "Acceptance")]
        public ActionResult Refuse()
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "驳回成功",
                Status = "n"
            };
            string s = base.Request.Form["idList"];
            string s2 = base.Request.Form["docId"];
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    int stageId = int.Parse(s);
                    int adoptDocId = int.Parse(s2);
                    PRO_PROJECT_STAGES pRO_PROJECT_STAGES = this.ProjectStageManage.Get((PRO_PROJECT_STAGES p) => p.ID == stageId);
                    PRO_PROJECT_FILES pRO_PROJECT_FILES = this.ProjectFilesManage.Get((PRO_PROJECT_FILES p) => p.ID == adoptDocId);
                    pRO_PROJECT_FILES.AcceptanceStatus = new int?(ClsDic.DicStatus["驳回"]);
                    this.ProjectFilesManage.Update(pRO_PROJECT_FILES);
                    this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                    {
                        FK_ProjectId = pRO_PROJECT_STAGES.FK_ProjectId,
                        CreateDate = DateTime.Now,
                        UserName = base.CurrentUser.Name,
                        UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                        MessageContent = "驳回了" + pRO_PROJECT_STAGES.StageTitle + "阶段任务文档：" + pRO_PROJECT_FILES.DocName
                    });
                    jsonHelper.Status = "y";
                    base.WriteLog(enumOperator.Audit, "驳回阶段任务：" + jsonHelper.Msg, enumLog4net.INFO);
                    transactionScope.Complete();
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "驳回失败，请稍后再试！";
                    base.WriteLog(enumOperator.Audit, "驳回阶段任务：", e);
                }
            }
            return base.Json(jsonHelper);
        }
    }
}
