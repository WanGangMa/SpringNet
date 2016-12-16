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
    public class ProjectDocumentController : BaseController
    {
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

        [UserAuthorize(ModuleAlias = "ProjectDoc", OperaAction = "View")]
        public ActionResult Index(int? id)
        {
            ActionResult result;
            try
            {
               
                //ProjectDocumentController.< Index > o__SiteContainer0.<> p__Site2.Target(ProjectDocumentController.< Index > o__SiteContainer0.<> p__Site2, base.ViewBag, id);
                result = base.View(this.BindList(id));
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "项目文档管理加载主页：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectDoc", OperaAction = "Detail")]
        public ActionResult Detail(int? id)
        {
            ActionResult result;
            try
            {
                PRO_PROJECT_FILES arg_B9_1;
                if ((arg_B9_1 = this.ProjectFilesManage.Get((PRO_PROJECT_FILES p) => (int?)p.ID == id)) == null)
                {
                    arg_B9_1 = new PRO_PROJECT_FILES
                    {
                        DocStyle = "project",
                        Fk_ForeignId = int.Parse(base.Request.QueryString["projectId"])
                    };
                }
                result = base.View(arg_B9_1);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "加载项目文档详情发生错误：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "ProjectDoc", OperaAction = "Add,Edit")]
        public ActionResult Save(PRO_PROJECT_FILES entity)
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
                    if (entity.ID <= 0)
                    {
                        entity.UploadDate = DateTime.Now;
                        entity.CreateUser = base.CurrentUser.Name;
                        entity.CreateIP = Utils.GetIP();
                        entity.FK_UserId = base.CurrentUser.Id;
                    }
                    else
                    {
                        entity.UploadDate = DateTime.Now;
                        flag = true;
                    }
                    if (!this.ProjectFilesManage.IsExist((PRO_PROJECT_FILES p) => p.DocName.Equals(entity.DocName) && p.ID != entity.ID && p.Fk_ForeignId == entity.Fk_ForeignId))
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            try
                            {
                                if (this.ProjectFilesManage.SaveOrUpdate(entity, flag))
                                {
                                    this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                                    {
                                        FK_ProjectId = entity.Fk_ForeignId,
                                        CreateDate = DateTime.Now,
                                        UserName = base.CurrentUser.Name,
                                        UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                                        MessageContent = flag ? ("更新了项目文档" + entity.DocName) : ("上传了项目文档" + entity.DocName)
                                    });
                                    jsonHelper.Status = "y";
                                }
                                else
                                {
                                    jsonHelper.Msg = "保存文档失败";
                                }
                                transactionScope.Complete();
                            }
                            catch (Exception e)
                            {
                                jsonHelper.Msg = "保存项目文档发生内部错误！";
                                base.WriteLog(enumOperator.None, "保存项目文档错误：", e);
                            }
                            goto IL_321;
                        }
                    }
                    jsonHelper.Msg = "项目文档已经存在，请不要重复添加!";
                }
                else
                {
                    jsonHelper.Msg = "未找到要操作的项目文档";
                }
                IL_321:
                if (flag)
                {
                    base.WriteLog(enumOperator.Edit, "修改项目文档[" + entity.DocName + "]，结果：" + jsonHelper.Msg, enumLog4net.INFO);
                }
                else
                {
                    base.WriteLog(enumOperator.Add, "添加项目文档[" + entity.DocName + "]，结果：" + jsonHelper.Msg, enumLog4net.INFO);
                }
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "保存项目文档发生内部错误！";
                base.WriteLog(enumOperator.None, "保存项目文档错误：", e2);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "ProjectDoc", OperaAction = "Remove")]
        public ActionResult Delete(string idList)
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Status = "n",
                Msg = "删除项目文档成功"
            };
            try
            {
                if (string.IsNullOrEmpty(idList))
                {
                    jsonHelper.Msg = "未找到要删除的项目文档";
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
                try
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        using (List<int>.Enumerator enumerator = id.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                int item = enumerator.Current;
                                PRO_PROJECT_FILES pRO_PROJECT_FILES = this.ProjectFilesManage.Get((PRO_PROJECT_FILES p) => p.ID == item);
                                this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                                {
                                    FK_ProjectId = pRO_PROJECT_FILES.Fk_ForeignId,
                                    CreateDate = DateTime.Now,
                                    UserName = base.CurrentUser.Name,
                                    UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                                    MessageContent = "删除了项目文档" + pRO_PROJECT_FILES.DocName
                                });
                            }
                        }
                        this.ProjectFilesManage.Delete((PRO_PROJECT_FILES p) => id.Contains(p.ID));
                        jsonHelper.Status = "y";
                        transactionScope.Complete();
                    }
                    base.WriteLog(enumOperator.Remove, "删除项目文档：" + jsonHelper.Msg, enumLog4net.WARN);
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "删除项目文档发生内部错误！";
                    base.WriteLog(enumOperator.Remove, "删除项目文档：", e);
                }
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "删除项目文档发生内部错误！";
                base.WriteLog(enumOperator.Remove, "删除项目文档：", e2);
            }
            return base.Json(jsonHelper);
        }

        private PageInfo BindList(int? projectId)
        {
            IQueryable<PRO_PROJECT_FILES> queryable = this.ProjectFilesManage.LoadAll((PRO_PROJECT_FILES p) => p.DocStyle == "project");
            if (projectId.HasValue && projectId > 0)
            {
                queryable = from p in queryable
                            where (int?)p.Fk_ForeignId == projectId
                            select p;
            }
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                queryable = from p in queryable
                            where p.DocName.Contains(this.keywords)
                            select p;
            }
            queryable = from p in queryable
                        orderby p.UploadDate
                        select p;
            PageInfo<PRO_PROJECT_FILES> pageInfo = this.ProjectFilesManage.Query(queryable, base.page, base.pagesize);
            var obj = (from p in pageInfo.List
                       select new
                       {
                           ID = p.ID,
                           DocName = p.DocName,
                           DocPath = p.DocPath,
                           UploadDate = p.UploadDate,
                           DocSize = p.DocSize,
                           Icon = FileHelper.GetFileIcon(p.DocFileExt)
                       }).ToList();
           
            return new PageInfo( pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(obj));
        }
    }
}
