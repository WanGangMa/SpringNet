using Common;
using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.ProManage.Controllers
{
    public class ProjectMissionController : BaseController
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

        private IProjectFilesManage ProjectFilesManage
        {
            get;
            set;
        }

        [UserAuthorize(ModuleAlias = "ProjectMission", OperaAction = "View")]
        public ActionResult Index()
        {
            ActionResult result;
            try
            {
                int CarriedOut = ClsDic.DicProject["进行中"];
                int CarriedOut2 = ClsDic.DicProject["延期"];
                int JoinStatus = ClsDic.DicStatus["通过"];
                base.ViewData["MissionList"] = (from p in this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == this.CurrentUser.Id && (p.PRO_PROJECT_STAGES.StageStatus == CarriedOut || p.PRO_PROJECT_STAGES.StageStatus == CarriedOut2) && p.JionStatus == JoinStatus)
                                                orderby p.PRO_PROJECT_STAGES.EndDate
                                                select p).ToList<PRO_PROJECT_TEAMS>();
                IQueryable<PRO_PROJECT_STAGES> source = base.CurrentUser.IsAdmin ? this.ProjectStageManage.LoadAll(null) : this.ProjectStageManage.LoadAll((PRO_PROJECT_STAGES p) => p.PRO_PROJECTS.Fk_DepartId == this.CurrentUser.DptInfo.ID);
                IQueryable<PRO_PROJECT_TEAMS> source2 = this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == this.CurrentUser.Id && p.JionStatus == JoinStatus);
                DateTime startWeek = DateTime.Now.AddDays((double)(1 - Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))));
                DateTime endWeek = startWeek.AddDays(6.0);
                int count = (from p in source
                             where p.StartDate >= startWeek && p.StartDate <= endWeek
                             select p).ToList<PRO_PROJECT_STAGES>().Count;
                int count2 = (from p in source2
                              where p.PRO_PROJECT_STAGES.StartDate >= startWeek && p.PRO_PROJECT_STAGES.StartDate <= endWeek
                              select p).ToList<PRO_PROJECT_TEAMS>().Count;
                base.ViewData["StageCurrentWeek"] = count;
                base.ViewData["TeamCurrentWeek"] = count2;
                base.ViewData["TeamCurrentWeek/StageCurrentWeek"] = ((count == 0) ? "0" : ((double)count2 / ((double)count * 1.0) * 100.0).ToString());
                int count3 = (from p in source
                              where p.StartDate.Month == DateTime.Now.Month
                              select p).ToList<PRO_PROJECT_STAGES>().Count;
                int count4 = (from p in source2
                              where p.PRO_PROJECT_STAGES.StartDate.Month == DateTime.Now.Month
                              select p).ToList<PRO_PROJECT_TEAMS>().Count;
                base.ViewData["StageCurrentMonth"] = count3;
                base.ViewData["TeamCurrentMonth"] = count4;
                base.ViewData["TeamCurrentMonth/StageCurrentMonth"] = ((count3 <= 0) ? "0" : ((double)count4 / ((double)count3 * 1.0) * 100.0).ToString());
                int count5 = source.ToList<PRO_PROJECT_STAGES>().Count;
                int count6 = source2.ToList<PRO_PROJECT_TEAMS>().Count;
                base.ViewData["StageCurrentCount"] = count5;
                base.ViewData["TeamCurrentCount"] = count6;
                base.ViewData["TeamCurrentCount/StageCurrentCount"] = ((count5 <= 0) ? "0" : ((double)count6 / ((double)count5 * 1.0) * 100.0).ToString());
                List<PRO_PROJECT_FILES> source3 = this.ProjectFilesManage.LoadListAll((PRO_PROJECT_FILES p) => p.DocStyle == "projectstage" && p.FK_UserId == this.CurrentUser.Id);
                int count7 = source3.ToList<PRO_PROJECT_FILES>().Count;
                int count8 = (from p in source3
                              where p.AcceptanceStatus == ClsDic.DicStatus["通过"]
                              select p).ToList<PRO_PROJECT_FILES>().Count;
                base.ViewData["TeamDoc/CurrentDoc"] = ((count7 > 0) ? ((int)Math.Floor((double)count8 / ((double)count7 * 1.0) * 100.0)) : 0);
                base.ViewData["CurrentUser"] = this.UserManage.Get((SYS_USER p) => p.ID == this.CurrentUser.Id);
               
                //ProjectMissionController.< Index > o__SiteContainer0.<> p__Site1.Target(ProjectMissionController.< Index > o__SiteContainer0.<> p__Site1, base.ViewBag, base.keywords);
                result = base.View(this.BindList());
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "任务中心加载主页：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectMission", OperaAction = "Add")]
        public ActionResult UpStageFile()
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "文档提交成功，等待验收",
                Status = "n"
            };
            string text = base.Request.Form["DocName"];
            string docNewName = base.Request.Form["DocNewName"];
            string docPath = base.Request.Form["DocPath"];
            string docFileExt = base.Request.Form["DocFileExt"];
            string docSize = base.Request.Form["DocSize"];
            string s = base.Request.Form["Fk_ForeignId"];
            try
            {
                int ForeignId = int.Parse(s);
                PRO_PROJECT_STAGES pRO_PROJECT_STAGES = this.ProjectStageManage.Get((PRO_PROJECT_STAGES p) => p.ID == ForeignId);
                this.ProjectFilesManage.Save(new PRO_PROJECT_FILES
                {
                    DocStyle = "projectstage",
                    Fk_ForeignId = ForeignId,
                    FK_UserId = base.CurrentUser.Id,
                    DocName = text,
                    DocNewName = docNewName,
                    DocPath = docPath,
                    DocFileExt = docFileExt,
                    DocSize = docSize,
                    UploadDate = DateTime.Now,
                    CreateUser = base.CurrentUser.Name,
                    CreateIP = Utils.GetIP(),
                    AcceptanceStatus = new int?(ClsDic.DicStatus["等待审核"])
                });
                this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                {
                    FK_ProjectId = pRO_PROJECT_STAGES.FK_ProjectId,
                    CreateDate = DateTime.Now,
                    UserName = base.CurrentUser.Name,
                    UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                    MessageContent = "提交" + pRO_PROJECT_STAGES.StageTitle + "阶段任务文档：" + text
                });
                jsonHelper.Status = "y";
                base.WriteLog(enumOperator.Audit, "阶段任务提交文档：" + jsonHelper.Msg, enumLog4net.INFO);
            }
            catch (Exception e)
            {
                jsonHelper.Msg = "提交文档失败，请稍后再试！";
                base.WriteLog(enumOperator.Audit, "阶段任务提交文档：", e);
            }
            return base.Json(jsonHelper);
        }

        private PageInfo BindList()
        {
            IQueryable<PRO_PROJECT_TEAMS> queryable = this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == this.CurrentUser.Id);
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                queryable = from p in queryable
                            where p.PRO_PROJECT_STAGES.StageTitle.Contains(this.keywords) || p.PRO_PROJECT_STAGES.PRO_PROJECTS.ProjectTitle.Contains(this.keywords)
                            select p;
            }
            queryable = from p in queryable
                        orderby p.JionStatus descending
                        orderby p.PRO_PROJECT_STAGES.EndDate
                        select p;
            PageInfo<PRO_PROJECT_TEAMS> pageInfo = this.ProjectTeamManage.Query(queryable, base.pageindex, base.pagesize);
            var obj = (from p in pageInfo.List
                       select new
                       {
                           ID = p.ID,
                           ProjectID = p.PRO_PROJECT_STAGES.PRO_PROJECTS.ID,
                           ProjectTitle = p.PRO_PROJECT_STAGES.PRO_PROJECTS.ProjectTitle,
                           StageTitle = p.PRO_PROJECT_STAGES.StageTitle,
                           JionStatus = Tools.GetEnumText<enumStatusType>(p.JionStatus),
                           StageTimeLimit = p.PRO_PROJECT_STAGES.StageTimeLimit,
                           StageStatus = Tools.GetEnumText<enumProjectType>(p.PRO_PROJECT_STAGES.StageStatus),
                           JionDate = p.JionDate,
                           IsAcceptance = this.GetStageFileAcceptance(p.PRO_PROJECT_STAGES.ID)
                       }).ToList();
            
            return new PageInfo( pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(obj));
        }

        private string GetStageFileAcceptance(int stageId)
        {
            int acceptstatus = ClsDic.DicStatus["通过"];
            int waitstatus = ClsDic.DicStatus["等待审核"];
            IQueryable<PRO_PROJECT_FILES> source = this.ProjectFilesManage.LoadAll((PRO_PROJECT_FILES m) => m.DocStyle == "projectstage" && m.Fk_ForeignId == stageId && m.FK_UserId == this.CurrentUser.Id);
            if ((from p in source
                 where p.AcceptanceStatus == (int?)acceptstatus
                 select p).ToList<PRO_PROJECT_FILES>().Count > 0)
            {
                return "<i class=\"fa fa-check text-navy\"></i>";
            }
            if ((from p in source
                 where p.AcceptanceStatus == (int?)waitstatus
                 select p).ToList<PRO_PROJECT_FILES>().Count > 0)
            {
                return "<i class=\"fa fa-ellipsis-h text-info\"></i>";
            }
            return "<i class=\"fa fa-close text-danger\"></i>";
        }
    }
}
