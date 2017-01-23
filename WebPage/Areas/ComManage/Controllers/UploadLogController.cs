using Common;
using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.ComManage.Controllers
{
    public class UploadLogController : BaseController
    {


        IUploadManage UploadManage;
        public UploadLogController(IUploadManage UploadManage)
        {
            this.UploadManage = UploadManage;
        }
       

        [UserAuthorize(ModuleAlias = "UploadLog", OperaAction = "View")]
        public ActionResult Index()
        {
            ActionResult result;
            try
            {
                string text = base.Request.QueryString["fileExt"];

                base.ViewData["fileExt"] = text;
                result = base.View(this.BindList(text));
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "文件上传记录加载主页：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "UploadLog", OperaAction = "View")]
        public ActionResult Detail(string logId)
        {
            return base.View(this.UploadManage.Get((COM_UPLOAD p) => p.ID == logId) ?? new COM_UPLOAD());
        }

        private PageInfo BindList(string fileExt)
        {
            IQueryable<COM_UPLOAD> queryable = this.UploadManage.LoadAll(null);
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                queryable = from p in queryable
                            where p.UPNEWNAME.Contains(this.keywords) || p.UPOPEATOR.Contains(this.keywords) || p.UPOLDNAME.Contains(this.keywords)
                            select p;
            }
            if (!base.CurrentUser.IsAdmin)
            {
                string userid = base.CurrentUser.Id.ToString();
                queryable = from p in queryable
                            where p.FK_USERID == userid
                            select p;
            }
            queryable = from p in queryable
                        orderby p.UPTIME descending
                        select p;
            PageInfo<COM_UPLOAD> pageInfo = this.UploadManage.Query(queryable, base.pageindex, 28);
            var list = (from p in pageInfo.List
                        select new
                        {
                            ID = p.ID,
                            UPOPEATOR = p.UPOPEATOR,
                            UPNEWNAME = p.UPNEWNAME,
                            UPTIME = p.UPTIME,
                            SIZE = p.UPFILESIZE + p.UPFILEUNIT,
                            ICON = this.GetFileIcon(p.UPFILESUFFIX)
                        }).ToList();
            if (!string.IsNullOrEmpty(fileExt) && fileExt != null)
            {
                if (!(fileExt == "images"))
                {
                    if (!(fileExt == "videos"))
                    {
                        if (!(fileExt == "musics"))
                        {
                            if (!(fileExt == "docements"))
                            {
                                if (fileExt == "others")
                                {
                                    list = (from p in list
                                            where p.ICON == "fa fa-file" || p.ICON == "fa fa-file-zip-o"
                                            select p).ToList();
                                }
                            }
                            else
                            {
                                list = (from p in list
                                        where p.ICON == "fa fa-file-word-o" || p.ICON == "fa fa-file-excel-o" || p.ICON == "fa fa-file-powerpoint-o" || p.ICON == "fa fa-file-pdf-o" || p.ICON == "fa fa-file-text-o"
                                        select p).ToList();
                            }
                        }
                        else
                        {
                            list = (from p in list
                                    where p.ICON == "fa fa-music"
                                    select p).ToList();
                        }
                    }
                    else
                    {
                        list = (from p in list
                                where p.ICON == "fa fa-film"
                                select p).ToList();
                    }
                }
                else
                {
                    list = (from p in list
                            where p.ICON == "fa fa-image"
                            select p).ToList();
                }
            }

            return new PageInfo(pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(list));
        }

        private string GetFileIcon(string _fileExt)
        {
            List<string> list = (from p in ConfigurationManager.AppSettings["Image"].Trim(new char[]
            {
                ','
            }).Split(new string[]
            {
                ","
            }, StringSplitOptions.RemoveEmptyEntries)
                                 select p).ToList<string>();
            List<string> list2 = (from p in ConfigurationManager.AppSettings["Video"].Trim(new char[]
            {
                ','
            }).Split(new string[]
            {
                ","
            }, StringSplitOptions.RemoveEmptyEntries)
                                  select p).ToList<string>();
            List<string> list3 = (from p in ConfigurationManager.AppSettings["Music"].Trim(new char[]
            {
                ','
            }).Split(new string[]
            {
                ","
            }, StringSplitOptions.RemoveEmptyEntries)
                                  select p).ToList<string>();
            if (list.Contains(_fileExt.ToLower()))
            {
                return "fa fa-image";
            }
            if (list2.Contains(_fileExt.ToLower()))
            {
                return "fa fa-film";
            }
            if (list3.Contains(_fileExt.ToLower()))
            {
                return "fa fa-music";
            }
            string key;
            switch (key = _fileExt.ToLower())
            {
                case "doc":
                case "docx":
                    return "fa fa-file-word-o";
                case "xls":
                case "xlsx":
                    return "fa fa-file-excel-o";
                case "ppt":
                case "pptx":
                    return "fa fa-file-powerpoint-o";
                case "pdf":
                    return "fa fa-file-pdf-o";
                case "txt":
                    return "fa fa-file-text-o";
                case "zip":
                case "rar":
                    return "fa fa-file-zip-o";
            }
            return "fa fa-file";
        }
    }
}
