using Common;
using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.ProManage.Controllers
{
    public class ProjectStageController : BaseController
    {


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

        private IProjectMessage ProjectMessage
        {
            get;
            set;
        }

        private IProjectManage ProjectManage
        {
            get;
            set;
        }

        private IProjectTeamManage ProjectTeamManage
        {
            get;
            set;
        }

        [UserAuthorize(ModuleAlias = "ProjectStage", OperaAction = "View")]
        public ActionResult Index(int? id)
        {
            ActionResult result;
            try
            {
                //if (ProjectStageController.< Index > o__SiteContainer0.<> p__Site1 == null)
                //{
                //    ProjectStageController.< Index > o__SiteContainer0.<> p__Site1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Search", typeof(ProjectStageController), new CSharpArgumentInfo[]
                //    {
                //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                //    }));
                //}
                //ProjectStageController.< Index > o__SiteContainer0.<> p__Site1.Target(ProjectStageController.< Index > o__SiteContainer0.<> p__Site1, base.ViewBag, base.keywords);
                //if (ProjectStageController.< Index > o__SiteContainer0.<> p__Site2 == null)
                //{
                //    ProjectStageController.< Index > o__SiteContainer0.<> p__Site2 = CallSite<Func<CallSite, object, int?, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "ProjectId", typeof(ProjectStageController), new CSharpArgumentInfo[]
                //    {
                //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                //        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                //    }));
                //}
                //ProjectStageController.< Index > o__SiteContainer0.<> p__Site2.Target(ProjectStageController.< Index > o__SiteContainer0.<> p__Site2, base.ViewBag, id);
                result = base.View(this.BindList(id));
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "任务阶段管理加载主页：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectStage", OperaAction = "Detail")]
        public ActionResult Detail(int? id)
        {
            ActionResult result;
            try
            {
                PRO_PROJECT_STAGES arg_AE_1;
                if ((arg_AE_1 = this.ProjectStageManage.Get((PRO_PROJECT_STAGES p) => (int?)p.ID == id)) == null)
                {
                    arg_AE_1 = new PRO_PROJECT_STAGES
                    {
                        FK_ProjectId = int.Parse(base.Request.QueryString["projectId"])
                    };
                }
                result = base.View(arg_AE_1);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "加载阶段任务详情发生错误：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectStage", OperaAction = "Add,Edit")]
        public ActionResult Save(PRO_PROJECT_STAGES entity)
        {
            bool flag = false;
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "保存成功",
                Status = "n"
            };
            try
            {
                if (entity != null)
                {
                    PRO_PROJECTS pRO_PROJECTS = this.ProjectManage.Get((PRO_PROJECTS p) => p.ID == entity.FK_ProjectId);
                    if (entity.ID <= 0)
                    {
                        int num = (this.ProjectStageManage.LoadAll((PRO_PROJECT_STAGES p) => p.FK_ProjectId == entity.FK_ProjectId) == null || this.ProjectStageManage.LoadAll((PRO_PROJECT_STAGES p) => p.FK_ProjectId == entity.FK_ProjectId).Count<PRO_PROJECT_STAGES>() <= 0) ? 0 : this.ProjectStageManage.LoadAll((PRO_PROJECT_STAGES p) => p.FK_ProjectId == entity.FK_ProjectId).Sum((PRO_PROJECT_STAGES p) => p.StageTimeLimit);
                        if (num + entity.StageTimeLimit > pRO_PROJECTS.ProjectTimeLimit * 24)
                        {
                            jsonHelper.Msg = "阶段任务总工期超出项目工期（" + (this.ProjectStageManage.LoadAll((PRO_PROJECT_STAGES p) => p.FK_ProjectId == entity.FK_ProjectId).Sum((PRO_PROJECT_STAGES p) => p.StageTimeLimit) + entity.StageTimeLimit - pRO_PROJECTS.ProjectTimeLimit * 24) + " 小时）！";
                            return base.Json(jsonHelper);
                        }
                        entity.StageStatus = 1;
                        entity.IsOverTime = false;
                        entity.OverDays = 0;
                        entity.CreateDate = DateTime.Now;
                        entity.CreateUser = base.CurrentUser.Name;
                        entity.UpdateDate = DateTime.Now;
                        entity.UpdateUser = base.CurrentUser.Name;
                        entity.EndDate = entity.StartDate.AddHours((double)entity.StageTimeLimit);
                    }
                    else
                    {
                        entity.EndDate = entity.StartDate.AddHours((double)entity.StageTimeLimit);
                        entity.UpdateDate = DateTime.Now;
                        entity.UpdateUser = base.CurrentUser.Name;
                        flag = true;
                    }
                    if (!this.ProjectStageManage.IsExist((PRO_PROJECT_STAGES p) => p.StageTitle.Equals(entity.StageTitle) && p.ID != entity.ID && p.FK_ProjectId == entity.FK_ProjectId))
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            try
                            {
                                if (this.ProjectStageManage.SaveOrUpdate(entity, flag))
                                {
                                    this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                                    {
                                        FK_ProjectId = entity.FK_ProjectId,
                                        CreateDate = DateTime.Now,
                                        UserName = base.CurrentUser.Name,
                                        UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                                        MessageContent = flag ? ("更新了阶段任务" + entity.StageTitle) : ("发布了阶段任务" + entity.StageTitle)
                                    });
                                    if (pRO_PROJECTS.ProjectStatus == 0)
                                    {
                                        pRO_PROJECTS.ProjectStatus = 1;
                                        this.ProjectManage.Update(pRO_PROJECTS);
                                    }
                                    jsonHelper.Status = "y";
                                }
                                else
                                {
                                    jsonHelper.Msg = "保存任务信息失败！";
                                }
                                transactionScope.Complete();
                            }
                            catch (Exception e)
                            {
                                jsonHelper.Msg = "保存任务信息发生内部错误！";
                                base.WriteLog(enumOperator.None, "保存任务错误：", e);
                            }
                            goto IL_748;
                        }
                    }
                    jsonHelper.Msg = "任务已经存在，请不要重复添加!";
                }
                else
                {
                    jsonHelper.Msg = "未找到要操作的任务记录";
                }
                IL_748:
                if (flag)
                {
                    base.WriteLog(enumOperator.Edit, "修改任务[" + entity.StageTitle + "]，结果：" + jsonHelper.Msg, enumLog4net.INFO);
                }
                else
                {
                    base.WriteLog(enumOperator.Add, "添加任务[" + entity.StageTitle + "]，结果：" + jsonHelper.Msg, enumLog4net.INFO);
                }
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "保存任务信息发生内部错误！";
                base.WriteLog(enumOperator.None, "保存任务错误：", e2);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "ProjectStage", OperaAction = "Remove")]
        public ActionResult Delete(string idList)
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Status = "n",
                Msg = "删除任务成功"
            };
            try
            {
                if (string.IsNullOrEmpty(idList))
                {
                    jsonHelper.Msg = "未找到要删除的任务";
                    return base.Json(jsonHelper);
                }
                List<int> id = (from p in idList.Trim(new char[]
                {
                    ','
                }).Split(new string[]
                {
                    ","
                }, StringSplitOptions.RemoveEmptyEntries)
                                select int.Parse(p)).ToList<int>();
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        this.ProjectFilesManage.Delete((PRO_PROJECT_FILES p) => p.DocStyle == "stage" && id.Contains(p.Fk_ForeignId));
                        using (List<int>.Enumerator enumerator = id.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                int item = enumerator.Current;
                                PRO_PROJECT_STAGES pRO_PROJECT_STAGES = this.ProjectStageManage.Get((PRO_PROJECT_STAGES p) => p.ID == item);
                                this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                                {
                                    FK_ProjectId = pRO_PROJECT_STAGES.FK_ProjectId,
                                    CreateDate = DateTime.Now,
                                    UserName = base.CurrentUser.Name,
                                    UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                                    MessageContent = "删除了阶段任务" + pRO_PROJECT_STAGES.StageTitle
                                });
                            }
                        }
                        this.ProjectTeamManage.Delete((PRO_PROJECT_TEAMS p) => id.Contains(p.FK_StageId));
                        this.ProjectStageManage.Delete((PRO_PROJECT_STAGES p) => id.Contains(p.ID));
                        base.WriteLog(enumOperator.Remove, "删除任务：" + jsonHelper.Msg, enumLog4net.WARN);
                        jsonHelper.Status = "y";
                        transactionScope.Complete();
                    }
                    catch (Exception e)
                    {
                        jsonHelper.Msg = "删除任务发生内部错误！";
                        base.WriteLog(enumOperator.Remove, "删除任务：", e);
                    }
                }
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "删除任务发生内部错误！";
                base.WriteLog(enumOperator.Remove, "删除任务：", e2);
            }
            return base.Json(jsonHelper);
        }

        private PageInfo BindList(int? projectId)
        {
            IQueryable<PRO_PROJECT_STAGES> queryable = this.ProjectStageManage.LoadAll(null);
            if (projectId.HasValue && projectId > 0)
            {
                queryable = from p in queryable
                            where (int?)p.FK_ProjectId == projectId
                            select p;
            }
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                queryable = from p in queryable
                            where p.StageTitle.Contains(this.keywords)
                            select p;
            }
            queryable = from p in queryable
                        orderby p.OrderNumber
                        select p;
            PageInfo<PRO_PROJECT_STAGES> pageInfo = this.ProjectStageManage.Query(queryable, base.pageindex, base.pagesize);
            var obj = pageInfo.List.Select(delegate (PRO_PROJECT_STAGES p)
            {
                int arg_9B_0 = p.ID;
                string arg_9B_1 = p.StageTitle;
                int arg_9B_2 = p.NeedNumber;
                int arg_9B_3;
                if (p.PRO_PROJECT_TEAMS != null)
                {
                    arg_9B_3 = (from m in p.PRO_PROJECT_TEAMS
                                where m.JionStatus == ClsDic.DicStatus["通过"]
                                select m).ToList<PRO_PROJECT_TEAMS>().Count;
                }
                else
                {
                    arg_9B_3 = 0;
                }
                return new
                {
                    ID = arg_9B_0,
                    StageTitle = arg_9B_1,
                    NeedNumber = arg_9B_2,
                    JsonNumber = arg_9B_3,
                    StageTimeLimit = p.StageTimeLimit,
                    StageStatus = Tools.GetEnumText<enumProjectType>(p.StageStatus),
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    IsOverTime = p.IsOverTime ? ("<span class=\"btn btn-danger btn-xs p210\">已超时&nbsp;-&nbsp;" + p.OverDays + "小时</span>") : "<span class=\"btn btn-primary btn-xs p210\">正常</span>",
                    OrderNumber = p.OrderNumber
                };
            }).ToList();
            //if (ProjectStageController.< BindList > o__SiteContainer12.<> p__Site13 == null)
            //{
            //    ProjectStageController.< BindList > o__SiteContainer12.<> p__Site13 = CallSite<Func<CallSite, Type, int, int, int, object, PageInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(ProjectStageController), new CSharpArgumentInfo[]
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
