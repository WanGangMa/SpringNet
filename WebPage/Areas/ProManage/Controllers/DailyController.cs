using Common;
using Domain;
using Service.IService;
using System;
using System.Linq;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.ProManage.Controllers
{
    public class DailyController : BaseController
    {
        private IDailyManage DailyManage
        {
            get;
            set;
        }

        private IContentManage ContentManage
        {
            get;
            set;
        }

        [UserAuthorize(ModuleAlias = "Daily", OperaAction = "View")]
        public ActionResult Index()
        {
            int month = string.IsNullOrEmpty(base.Request.QueryString["month"]) ? DateTime.Now.Month : int.Parse(base.Request.QueryString["month"]);
            base.ViewData["week"] = this.GetWeek(month);
            base.ViewData["month"] = month;
            base.ViewData["DailyList"] = this.DailyManage.LoadAll((COM_DAILYS p) => p.FK_USERID == this.CurrentUser.Id && p.AddDate.Year == DateTime.Now.Year && p.AddDate.Month == month).ToList<COM_DAILYS>();
            return base.View();
        }

        [UserAuthorize(ModuleAlias = "Daily", OperaAction = "Detail")]
        public ActionResult Detail(int? id)
        {
            ActionResult result;
            try
            {
                COM_DAILYS entity = new COM_DAILYS();
                if (id.HasValue && id > 0)
                {
                    entity = this.DailyManage.Get((COM_DAILYS p) => (int?)p.ID == id);
                    base.ViewData["Content"] = (this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "COM_DAILYS") ?? new COM_CONTENT());
                }
                result = base.View(entity);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "日报管理加载详情", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [ValidateInput(false), UserAuthorize(ModuleAlias = "Daily", OperaAction = "Add,Edit")]
        public ActionResult Save(COM_DAILYS entity)
        {
            bool isEdit = false;
            int num = (base.Request["Content_Id"] == null) ? 0 : int.Parse(base.Request["Content_Id"].ToString());
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "日报提交成功",
                Status = "n"
            };
            try
            {
                string fK_RELATIONID;
                if (entity.ID <= 0)
                {
                    fK_RELATIONID = Guid.NewGuid().ToString();
                    entity.FK_USERID = base.CurrentUser.Id;
                    entity.FK_RELATIONID = fK_RELATIONID;
                    entity.AddDate = DateTime.Now;
                    entity.LastEditDate = DateTime.Now;
                    entity.DailySubIP = Utils.GetIP();
                }
                else
                {
                    entity.LastEditDate = DateTime.Now;
                    entity.DailySubIP = Utils.GetIP();
                    fK_RELATIONID = entity.FK_RELATIONID;
                    isEdit = true;
                }
                if (!this.DailyManage.SaveOrUpdate(entity, isEdit))
                {
                    jsonHelper.Msg = "提交日报失败";
                }
                else
                {
                    if (num <= 0)
                    {
                        this.ContentManage.Save(new COM_CONTENT
                        {
                            CONTENT = base.Request["Content"],
                            FK_RELATIONID = fK_RELATIONID,
                            FK_TABLE = "COM_DAILYS",
                            CREATEDATE = DateTime.Now
                        });
                    }
                    else
                    {
                        this.ContentManage.Update(new COM_CONTENT
                        {
                            ID = num,
                            CONTENT = base.Request["Content"],
                            FK_RELATIONID = fK_RELATIONID,
                            FK_TABLE = "COM_DAILYS",
                            CREATEDATE = DateTime.Now
                        });
                    }
                    jsonHelper.Status = "y";
                }
            }
            catch (Exception e)
            {
                jsonHelper.Msg = "提交日报发生内部错误";
                base.WriteLog(enumOperator.None, "保存日报发生内部错误", e);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "Daily", OperaAction = "Detail")]
        public ActionResult ViewDetail(int id)
        {
            ActionResult result;
            try
            {
                COM_DAILYS entity = this.DailyManage.Get((COM_DAILYS p) => p.ID == id);
                base.ViewData["Content"] = ((this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "COM_DAILYS") == null) ? "" : this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "COM_DAILYS").CONTENT);
                result = base.View(entity);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "日报管理加载详情", ex);
                throw ex.InnerException;
            }
            return result;
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
