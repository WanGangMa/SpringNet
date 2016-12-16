using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context.Support;
using System.Web.Mvc;
using Common;
using Domain;
using Service.IService;
using WebPage.Controllers;
using System.Configuration;

namespace WebPage.Areas.SysManage.Controllers
{
    public class HomeController : BaseController
    {
       
        private IModuleManage ModuleManage
        {
            get;
            set;
        }

        private IProjectTeamManage ProjectTeamManage
        {
            get;
            set;
        }

        private IMailinManage MailinManage
        {
            get;
            set;
        }
        private IUserOnlineManage UserOnlineManage
        {
            get;
            set;
        }

        private IDepartmentManage DepartmentManage
        {
            get;
            set;
        }

        private IWorkAttendanceManage WorkAttendanceManage
        {
            get;
            set;
        }

        public ActionResult Index()
        {
            base.ViewData["Module"] = this.ModuleManage.GetModule(base.CurrentUser.Id, base.CurrentUser.Permissions, base.CurrentUser.System_Id);
            int CarriedOut = ClsDic.DicProject["进行中"];
            int CarriedOut2 = ClsDic.DicProject["延期"];
            int JoinStatus = ClsDic.DicStatus["通过"];
            base.ViewData["MissionList"] = (from p in this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == this.CurrentUser.Id && (p.PRO_PROJECT_STAGES.StageStatus == CarriedOut || p.PRO_PROJECT_STAGES.StageStatus == CarriedOut2) && p.JionStatus == JoinStatus)
                                            orderby p.PRO_PROJECT_STAGES.EndDate
                                            select p).ToList<PRO_PROJECT_TEAMS>();
            int MailInbox = ClsDic.DicMailType["收件箱"];
            int MailUnRead = ClsDic.DicMailReadStatus["未读"];
            base.ViewData["MailInBox"] = this.MailinManage.LoadListAll((MAIL_INBOX p) => p.Recipient.Contains(this.CurrentUser.LogName + this.EmailDomain) && p.ReadStatus == MailUnRead && p.MailType == MailInbox);
            base.ViewData["Contacts"] = this.Contacts();
            return base.View(base.CurrentUser);
        }

        public ActionResult Default()
        {
            int CarriedOut = ClsDic.DicProject["进行中"];
            int CarriedOut2 = ClsDic.DicProject["延期"];
            int JoinStatus = ClsDic.DicStatus["通过"];
            base.ViewData["MissionList"] = (from p in this.ProjectTeamManage.LoadAll((PRO_PROJECT_TEAMS p) => p.FK_UserId == this.CurrentUser.Id && (p.PRO_PROJECT_STAGES.StageStatus == CarriedOut || p.PRO_PROJECT_STAGES.StageStatus == CarriedOut2) && p.JionStatus == JoinStatus)
                                            orderby p.PRO_PROJECT_STAGES.EndDate
                                            select p).ToList<PRO_PROJECT_TEAMS>();
            int MailInbox = ClsDic.DicMailType["收件箱"];
            int MailUnRead = ClsDic.DicMailReadStatus["未读"];
            base.ViewData["MailInBoxCount"] = this.MailinManage.LoadListAll((MAIL_INBOX p) => p.Recipient.Contains(this.CurrentUser.LogName + this.EmailDomain) && p.ReadStatus == MailUnRead && p.MailType == MailInbox).Count;
            int month = string.IsNullOrEmpty(base.Request.QueryString["month"]) ? DateTime.Now.Month : int.Parse(base.Request.QueryString["month"]);
            base.ViewData["week"] = this.GetWeek(month);
            base.ViewData["month"] = month;
            base.ViewData["AttendanceList"] = this.WorkAttendanceManage.LoadAll((COM_WORKATTENDANCE p) => p.FK_UserId == this.CurrentUser.Id && p.CreateDate.Year == DateTime.Now.Year && p.CreateDate.Month == month).ToList<COM_WORKATTENDANCE>();
            return base.View(base.CurrentUser);
        }

        [ValidateAntiForgeryToken]
        public ActionResult Attendance()
        {
            bool isEdit = false;
            string msg = string.Empty;
            JsonHelper jsonHelper = new JsonHelper
            {
                Status = "n"
            };
            try
            {
                DateTime dateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + ConfigurationManager.AppSettings["OnDutyAM"]);
                DateTime dateTime2 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + ConfigurationManager.AppSettings["OffDutyAM"]);
                DateTime dateTime3 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + ConfigurationManager.AppSettings["OnDutyPM"]);
                DateTime dateTime4 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " " + ConfigurationManager.AppSettings["OffDutyPM"]);
                COM_WORKATTENDANCE cOM_WORKATTENDANCE = this.WorkAttendanceManage.LoadAll((COM_WORKATTENDANCE p) => p.FK_UserId == this.CurrentUser.Id && p.CreateDate.Year == DateTime.Now.Year && p.CreateDate.Month == DateTime.Now.Month && p.CreateDate.Day == DateTime.Now.Day).FirstOrDefault<COM_WORKATTENDANCE>();
                if (cOM_WORKATTENDANCE != null && cOM_WORKATTENDANCE.ID > 0)
                {
                    isEdit = true;
                }
                else
                {
                    cOM_WORKATTENDANCE = new COM_WORKATTENDANCE();
                }
                if (!cOM_WORKATTENDANCE.Is_SiginAM)
                {
                    cOM_WORKATTENDANCE.FK_UserId = base.CurrentUser.Id;
                    cOM_WORKATTENDANCE.Is_SiginAM = true;
                    cOM_WORKATTENDANCE.Is_SigOutAM = false;
                    cOM_WORKATTENDANCE.SiginAMDate = DateTime.Now;
                    cOM_WORKATTENDANCE.SigOutAMDate = DateTime.Now;
                    cOM_WORKATTENDANCE.Is_SiginPM = false;
                    cOM_WORKATTENDANCE.Is_SigOutPM = false;
                    cOM_WORKATTENDANCE.SiginPM = DateTime.Now;
                    cOM_WORKATTENDANCE.SigOutPM = DateTime.Now;
                    if (DateTime.Now > dateTime)
                    {
                        cOM_WORKATTENDANCE.IsLateAM = true;
                        cOM_WORKATTENDANCE.LateAMMinutes = (int)Math.Round((DateTime.Now - dateTime).TotalMinutes);
                        msg = "上午已签到：迟到" + cOM_WORKATTENDANCE.LateAMMinutes + "分钟";
                    }
                    else
                    {
                        cOM_WORKATTENDANCE.IsLateAM = false;
                        cOM_WORKATTENDANCE.LateAMMinutes = 0;
                        msg = "上午已签到";
                    }
                    cOM_WORKATTENDANCE.IsEarlyOutAM = false;
                    cOM_WORKATTENDANCE.EarlyOutAMMinutes = 0;
                    cOM_WORKATTENDANCE.IsLatePM = false;
                    cOM_WORKATTENDANCE.LatePMMinutes = 0;
                    cOM_WORKATTENDANCE.IsEarlyOutPM = false;
                    cOM_WORKATTENDANCE.EarlyOutPMMinutes = 0;
                    cOM_WORKATTENDANCE.WorkHours = 0.0;
                    cOM_WORKATTENDANCE.SigIP = Utils.GetIP();
                    cOM_WORKATTENDANCE.CreateDate = DateTime.Now;
                }
                else if (Utils.GetIP() != cOM_WORKATTENDANCE.SigIP)
                {
                    jsonHelper.Msg = "同一天考勤IP不允许更换！";
                }
                else if (!cOM_WORKATTENDANCE.Is_SigOutAM)
                {
                    cOM_WORKATTENDANCE.Is_SigOutAM = true;
                    cOM_WORKATTENDANCE.SigOutAMDate = DateTime.Now;
                    cOM_WORKATTENDANCE.Is_SiginPM = false;
                    cOM_WORKATTENDANCE.Is_SigOutPM = false;
                    cOM_WORKATTENDANCE.SiginPM = DateTime.Now;
                    cOM_WORKATTENDANCE.SigOutPM = DateTime.Now;
                    if (DateTime.Now < dateTime2)
                    {
                        cOM_WORKATTENDANCE.IsEarlyOutAM = true;
                        cOM_WORKATTENDANCE.EarlyOutAMMinutes = (int)Math.Round((dateTime2 - DateTime.Now).TotalMinutes);
                        msg = "上午已签退：早退" + cOM_WORKATTENDANCE.EarlyOutAMMinutes + "分钟";
                    }
                    else
                    {
                        cOM_WORKATTENDANCE.IsEarlyOutAM = false;
                        cOM_WORKATTENDANCE.EarlyOutAMMinutes = 0;
                        msg = "上午已签退";
                    }
                    cOM_WORKATTENDANCE.IsLatePM = false;
                    cOM_WORKATTENDANCE.LatePMMinutes = 0;
                    cOM_WORKATTENDANCE.IsEarlyOutPM = false;
                    cOM_WORKATTENDANCE.EarlyOutPMMinutes = 0;
                    cOM_WORKATTENDANCE.WorkHours = 0.0;
                }
                else if (!cOM_WORKATTENDANCE.Is_SiginPM)
                {
                    cOM_WORKATTENDANCE.Is_SiginPM = true;
                    cOM_WORKATTENDANCE.Is_SigOutPM = false;
                    cOM_WORKATTENDANCE.SiginPM = DateTime.Now;
                    cOM_WORKATTENDANCE.SigOutPM = DateTime.Now;
                    if (DateTime.Now > dateTime3)
                    {
                        cOM_WORKATTENDANCE.IsLatePM = true;
                        cOM_WORKATTENDANCE.LatePMMinutes = (int)Math.Round((DateTime.Now - dateTime3).TotalMinutes);
                        msg = "下午已签到：迟到" + cOM_WORKATTENDANCE.LatePMMinutes + "分钟";
                    }
                    else
                    {
                        cOM_WORKATTENDANCE.IsLatePM = false;
                        cOM_WORKATTENDANCE.LatePMMinutes = 0;
                        msg = "下午已签到";
                    }
                    cOM_WORKATTENDANCE.IsEarlyOutPM = false;
                    cOM_WORKATTENDANCE.EarlyOutPMMinutes = 0;
                    cOM_WORKATTENDANCE.WorkHours = 0.0;
                }
                else
                {
                    cOM_WORKATTENDANCE.Is_SigOutPM = true;
                    cOM_WORKATTENDANCE.SigOutPM = DateTime.Now;
                    if (DateTime.Now < dateTime4)
                    {
                        cOM_WORKATTENDANCE.IsEarlyOutPM = true;
                        cOM_WORKATTENDANCE.EarlyOutPMMinutes = (int)Math.Round((dateTime4 - DateTime.Now).TotalMinutes);
                        msg = "下午已签退：早退" + cOM_WORKATTENDANCE.EarlyOutPMMinutes + "分钟";
                    }
                    else
                    {
                        cOM_WORKATTENDANCE.IsEarlyOutPM = false;
                        cOM_WORKATTENDANCE.EarlyOutPMMinutes = 0;
                        msg = "下午已签退";
                    }
                    cOM_WORKATTENDANCE.WorkHours = (cOM_WORKATTENDANCE.SigOutAMDate - cOM_WORKATTENDANCE.SiginAMDate).TotalHours + (cOM_WORKATTENDANCE.SigOutPM - cOM_WORKATTENDANCE.SiginPM).TotalHours;
                }
                if (this.WorkAttendanceManage.SaveOrUpdate(cOM_WORKATTENDANCE, isEdit))
                {
                    jsonHelper.Msg = msg;
                    jsonHelper.Status = "y";
                }
                else
                {
                    jsonHelper.Msg = "发生内部错误，请稍后再试！";
                }
            }
            catch (Exception e)
            {
                jsonHelper.Msg = "发生内部错误，请稍后再试！";
                base.WriteLog(enumOperator.None, "考勤发生内部错误", e);
            }
            return base.Json(jsonHelper);
        }


        private object GetDepartUsers(string departId)
        {
            List<string> departs = (from p in getAllChildrenDeptIds(departId) // DepartmentManage.LoadAll(p => p.PARENTID == departId)
                                    orderby p.SHOWORDER
                                    select p.ID).ToList();
            departs.Add(departId); //加上顶层id，否则在顶层部门用户不显示
            var users = UserManage.LoadListAll(p => p.ID != CurrentUser.Id && departs.Any(e => e == p.DPTID))
                .OrderBy(p => p.LEVELS).ThenBy(p => p.CREATEDATE);
            var ret = users
                .Select(p =>
                {
                    return new
                    {
                        ID = p.ID,
                        FaceImg = (string.IsNullOrEmpty(p.FACE_IMG) ? ("/Pro/Project/User_Default_Avatat?name=" + p.NAME.Substring(0, 1)) : p.FACE_IMG),
                        NAME = p.NAME,
                        InsideEmail = p.ACCOUNT + base.EmailDomain,
                        LEVELS = p.LEVELS,
                        ConnectId = ((UserOnlineManage.LoadAll(m => m.FK_UserId == p.ID).FirstOrDefault() == null) ? "" : UserOnlineManage.LoadAll(m => m.FK_UserId == p.ID).FirstOrDefault().ConnectId),
                        IsOnline = (UserOnlineManage.LoadAll(m => m.FK_UserId == p.ID).FirstOrDefault() != null && UserOnlineManage.LoadAll(m => m.FK_UserId == p.ID).FirstOrDefault().IsOnline)
                    };
                })
                .OrderBy(p => p.IsOnline);
            return ret.ToList();
        }

        private object Contacts()
        {
            var obj = from m in (from m in DepartmentManage.LoadAll(m => m.BUSINESSLEVEL == 1)
                                 orderby m.SHOWORDER
                                 select m).ToList()
                      select new
                      {
                          ID = m.ID,
                          DepartName = m.NAME,
                          UserList = GetDepartUsers(m.ID)
                      };
            return JsonConverter.JsonClass(obj);
        }

        /// <summary>
        /// 循环查找部门的下属部门
        /// </summary>
        /// <param name="topDeptId"></param>
        /// <returns></returns>
        private List<SYS_DEPARTMENT> getAllChildrenDeptIds(string topDeptId)
        {
            List<SYS_DEPARTMENT> ret = new List<SYS_DEPARTMENT>();

            var depts = DepartmentManage.LoadAll(p => p.PARENTID == topDeptId);
            ret.AddRange(depts);

            depts.ToList().ForEach(d =>
            {
                ret.AddRange(getAllChildrenDeptIds(d.ID));
            });


            return ret;
        }
     
        private int GetWeek(int month)
        {
            int result = 0;
            string text = Convert.ToDateTime(string.Concat(new object[]
            {
                DateTime.Now.Year,
                "-",
                month,
                "-01"
            })).DayOfWeek.ToString();
            string key;
            switch (key = text)
            {
                case "Monday":
                    result = 1;
                    break;
                case "Tuesday":
                    result = 2;
                    break;
                case "Wednesday":
                    result = 3;
                    break;
                case "Thursday":
                    result = 4;
                    break;
                case "Friday":
                    result = 5;
                    break;
                case "Saturday":
                    result = 6;
                    break;
                case "Sunday":
                    result = 7;
                    break;
            }
            return result;
        }

    }
}