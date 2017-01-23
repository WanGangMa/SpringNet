using Common;
using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.ProManage.Controllers
{
    public class ProjectController : BaseController
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

[UserAuthorize(ModuleAlias = "Project", OperaAction = "View")]
public ActionResult Index()
{
    ActionResult result;
    try
    {
        string text = base.Request.QueryString["ProjectType"];
        base.ViewData["ProjectType"] = text;
        //if (ProjectController.< Index > o__SiteContainer0.<> p__Site1 == null)
        //{
        //    ProjectController.< Index > o__SiteContainer0.<> p__Site1 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Search", typeof(ProjectController), new CSharpArgumentInfo[]
        //    {
        //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
        //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
        //    }));
        //}
        //ProjectController.< Index > o__SiteContainer0.<> p__Site1.Target(ProjectController.< Index > o__SiteContainer0.<> p__Site1, base.ViewBag, base.keywords);
        base.ViewData["ProjectTypeList"] = Tools.BindEnumsList(typeof(enumProjectType));
        result = base.View(this.BindList(text));
    }
    catch (Exception ex)
    {
        base.WriteLog(enumOperator.Select, "项目管理加载主页：", ex);
        throw ex.InnerException;
    }
    return result;
}

[UserAuthorize(ModuleAlias = "Project", OperaAction = "Detail")]
public ActionResult Detail(int? id)
{
    ActionResult result;
    try
    {
        PRO_PROJECTS entity = new PRO_PROJECTS();
        if (id.HasValue && id > 0)
        {
            entity = this.ProjectManage.Get((PRO_PROJECTS p) => (int?)p.ID == id);
            base.ViewData["ProjectDescribe"] = (this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "PRO_PROJECTS") ?? new COM_CONTENT());
        }
        List<SYS_BUSSINESSCUSTOMER> source = base.CurrentUser.IsAdmin ? this.BussinessCustomerManage.LoadListAll(null) : this.BussinessCustomerManage.LoadListAll((SYS_BUSSINESSCUSTOMER p) => p.Fk_DepartId == this.CurrentUser.DptInfo.ID);
        base.ViewData["CustomerList"] = JsonConverter.JsonClass(source.Select(delegate (SYS_BUSSINESSCUSTOMER p)
        {
            return new
            {
                ID = p.ID,
                CompanyName = p.CompanyName,
                IsValidate = p.IsValidate,
                CompanyProvince = this.CodeAreaManage.Get((SYS_CODE_AREA m) => m.ID == p.CompanyProvince).NAME,
                CompanyCity = this.CodeAreaManage.Get((SYS_CODE_AREA m) => m.ID == p.CompanyCity).NAME,
                CompanyArea = this.CodeAreaManage.Get((SYS_CODE_AREA m) => m.ID == p.CompanyArea).NAME,
                CompanyTel = p.CompanyTel,
                ChargePersionName = p.ChargePersionName,
                CustomerStyle = this.GetCustomerStyle(p.CustomerStyle)
            };
        }).ToList());
        result = base.View(entity);
    }
    catch (Exception ex)
    {
        base.WriteLog(enumOperator.Select, "加载项目详情发生错误：", ex);
        throw ex.InnerException;
    }
    return result;
}

[ValidateInput(false), UserAuthorize(ModuleAlias = "Project", OperaAction = "Add,Edit")]
public ActionResult Save(PRO_PROJECTS entity)
{
    bool flag = false;
    JsonHelper jsonHelper = new JsonHelper
    {
        Msg = "保存成功,请添加项目文档",
        Status = "n"
    };
    try
    {
        if (entity != null)
        {
            if (entity.Fk_BussinessCustomer <= 0)
            {
                jsonHelper.Msg = "请选择客户";
                return base.Json(jsonHelper);
            }
            int num = (base.Request["ContentId"] == null) ? 0 : int.Parse(base.Request["ContentId"].ToString());
            string fK_RELATIONID;
            if (entity.ID <= 0)
            {
                fK_RELATIONID = Guid.NewGuid().ToString();
                entity.FK_RELATIONID = fK_RELATIONID;
                entity.Fk_DepartId = ((base.CurrentUser.DptInfo == null) ? "" : base.CurrentUser.DptInfo.ID);
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
                entity.ProjectStatus = 0;
                entity.Fk_UserId = base.CurrentUser.Id;
                entity.EndDate = entity.StartDate.AddDays((double)entity.ProjectTimeLimit);
            }
            else
            {
                entity.EndDate = entity.StartDate.AddDays((double)entity.ProjectTimeLimit);
                fK_RELATIONID = entity.FK_RELATIONID;
                entity.UpdateDate = DateTime.Now;
                flag = true;
            }
            if (!this.ProjectManage.IsExist((PRO_PROJECTS p) => p.ProjectTitle.Equals(entity.ProjectTitle) && p.ID != entity.ID && p.Fk_DepartId == entity.Fk_DepartId))
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        if (this.ProjectManage.SaveOrUpdate(entity, flag))
                        {
                            if (num <= 0)
                            {
                                this.ContentManage.Save(new COM_CONTENT
                                {
                                    CONTENT = base.Request["Content"],
                                    FK_RELATIONID = fK_RELATIONID,
                                    FK_TABLE = "PRO_PROJECTS",
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
                                    FK_TABLE = "PRO_PROJECTS",
                                    CREATEDATE = DateTime.Now
                                });
                            }
                            this.ProjectMessage.Save(new PRO_PROJECT_MESSAGE
                            {
                                FK_ProjectId = entity.ID,
                                CreateDate = DateTime.Now,
                                UserName = base.CurrentUser.Name,
                                UserFace = string.IsNullOrEmpty(base.CurrentUser.Face_Img) ? ("/Pro/Project/User_Default_Avatat?name=" + base.CurrentUser.Name.Substring(0, 1).ToUpper()) : base.CurrentUser.Face_Img,
                                MessageContent = flag ? ("更新了项目" + entity.ProjectTitle) : ("发布了新项目" + entity.ProjectTitle)
                            });
                            jsonHelper.ReUrl = "/Pro/ProjectDocument?id=" + entity.ID;
                            jsonHelper.Status = "y";
                        }
                        else
                        {
                            jsonHelper.Msg = "保存项目信息失败！";
                        }
                        transactionScope.Complete();
                    }
                    catch (Exception e)
                    {
                        jsonHelper.Msg = "保存项目信息发生内部错误！";
                        base.WriteLog(enumOperator.None, "保存项目错误：", e);
                    }
                    goto IL_515;
                }
            }
            jsonHelper.Msg = "项目已经存在，请不要重复添加!";
        }
        else
        {
            jsonHelper.Msg = "未找到要操作的项目记录";
        }
        IL_515:
        if (flag)
        {
            base.WriteLog(enumOperator.Edit, "修改项目[" + entity.ProjectTitle + "]，结果：" + jsonHelper.Msg, enumLog4net.INFO);
        }
        else
        {
            base.WriteLog(enumOperator.Add, "添加项目[" + entity.ProjectTitle + "]，结果：" + jsonHelper.Msg, enumLog4net.INFO);
        }
    }
    catch (Exception e2)
    {
        jsonHelper.Msg = "保存项目信息发生内部错误！";
        base.WriteLog(enumOperator.None, "保存项目错误：", e2);
    }
    return base.Json(jsonHelper);
}

[UserAuthorize(ModuleAlias = "Project", OperaAction = "Remove")]
public ActionResult Delete(string idList)
{
    JsonHelper jsonHelper = new JsonHelper
    {
        Status = "n",
        Msg = "删除项目成功"
    };
    try
    {
        if (string.IsNullOrEmpty(idList))
        {
            jsonHelper.Msg = "未找到要删除的项目";
            return base.Json(jsonHelper);
        }
        List<int> idList1 = (from p in idList.Trim(new char[]
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
                using (List<int>.Enumerator enumerator = idList1.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        int id = enumerator.Current;
                        this.ProjectFilesManage.Delete((PRO_PROJECT_FILES p) => p.DocStyle == "project" && p.Fk_ForeignId == id);
                        List<PRO_PROJECT_STAGES> list = this.ProjectStageManage.LoadListAll((PRO_PROJECT_STAGES p) => p.FK_ProjectId == id);
                        if (list != null && list.Count > 0)
                        {
                            using (List<PRO_PROJECT_STAGES>.Enumerator enumerator2 = list.GetEnumerator())
                            {
                                while (enumerator2.MoveNext())
                                {
                                    PRO_PROJECT_STAGES projectstage = enumerator2.Current;
                                    this.ProjectTeamManage.Delete((PRO_PROJECT_TEAMS p) => p.FK_StageId == projectstage.ID);
                                    this.ProjectFilesManage.Delete((PRO_PROJECT_FILES p) => p.DocStyle == "stage" && p.Fk_ForeignId == projectstage.ID);
                                }
                            }
                        }
                        this.ProjectStageManage.Delete((PRO_PROJECT_STAGES p) => p.FK_ProjectId == id);
                        this.ProjectMessage.Delete((PRO_PROJECT_MESSAGE p) => p.FK_ProjectId == id);
                    }
                }
                this.ProjectManage.Delete((PRO_PROJECTS p) => idList1.Contains(p.ID));
                base.WriteLog(enumOperator.Remove, "删除项目：" + jsonHelper.Msg, enumLog4net.WARN);
                jsonHelper.Status = "y";
                transactionScope.Complete();
            }
            catch (Exception e)
            {
                jsonHelper.Msg = "删除项目发生内部错误！";
                base.WriteLog(enumOperator.Remove, "删除项目：", e);
            }
        }
    }
    catch (Exception e2)
    {
        jsonHelper.Msg = "删除客户发生内部错误！";
        base.WriteLog(enumOperator.Remove, "删除客户：", e2);
    }
    return base.Json(jsonHelper);
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
                orderby p.ProjectLevels, p.EndDate
                select p;
    PageInfo<PRO_PROJECTS> pageInfo = this.ProjectManage.Query(queryable, base.pageindex, base.pagesize);
    var obj = pageInfo.List.Select(delegate (PRO_PROJECTS p)
    {
        return new
        {
            ID = p.ID,
            ProjectTitle = p.ProjectTitle,
            ProjectStatus = Tools.GetEnumText<enumProjectType>(p.ProjectStatus),
            ProjectLevels = Tools.GetEnumText<enumProjectLevels>(p.ProjectLevels),
            ProjectManager = (this.UserManage.Get((SYS_USER m) => m.ID == p.Fk_UserId) == null) ? "" : this.UserManage.Get((SYS_USER m) => m.ID == p.Fk_UserId).NAME,
            BussinessCustomer = (this.BussinessCustomerManage.Get((SYS_BUSSINESSCUSTOMER m) => m.ID == p.Fk_BussinessCustomer) == null) ? "" : this.BussinessCustomerManage.Get((SYS_BUSSINESSCUSTOMER m) => m.ID == p.Fk_BussinessCustomer).CompanyName,
            ProjectMoney = p.ProjectMoney,
            ContractCode = p.ContractCode,
            ContractFile = p.ContractFile,
            SignDate = p.SignDate.ToString("yyy年MM月dd日"),
            ProjectTimeLimit = p.ProjectTimeLimit,
            StartDate = p.StartDate.ToString("yyyy年MM月dd日"),
            EndDate = p.EndDate.ToString("yyyy年MM月dd日"),
            Teams = this.GetTeams(p.PRO_PROJECT_STAGES.ToList<PRO_PROJECT_STAGES>()),
            Progerss = this.GetProgress(p.PRO_PROJECT_STAGES.ToList<PRO_PROJECT_STAGES>())
        };
    }).ToList();
    //if (ProjectController.< BindList > o__SiteContainer19.<> p__Site1a == null)
    //{
    //    ProjectController.< BindList > o__SiteContainer19.<> p__Site1a = CallSite<Func<CallSite, Type, int, int, int, object, PageInfo>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof(ProjectController), new CSharpArgumentInfo[]
    //    {
    //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
    //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
    //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
    //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
    //                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
    //    }));
    //}
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

public FileContentResult User_Default_Avatat(string name)
{
    MemoryStream memoryStream = new user_avatat().Create(name);
    base.Response.ClearContent();
    return base.File(memoryStream.ToArray(), "image/png");
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

private string GetCustomerStyle(int codevalue)
{
    string nAMETEXT = this.CodeManage.Get((SYS_CODE p) => p.CODEVALUE == codevalue.ToString() && p.CODETYPE == "LXRLX").NAMETEXT;
    switch (codevalue)
    {
        case 1:
            return "<span class=\"btn btn-danger btn-xs p210\">" + nAMETEXT + "</span>";
        case 2:
            return "<span class=\"btn btn-warning btn-xs p210\">" + nAMETEXT + "</span>";
        default:
            return "<span class=\"btn btn-white btn-xs p210\">" + nAMETEXT + "</span>";
    }
}
	}
}
