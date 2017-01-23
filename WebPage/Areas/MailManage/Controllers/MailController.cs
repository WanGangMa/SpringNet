using Common;
using Domain;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using WebPage.Controllers;

namespace WebPage.Areas.MailManage.Controllers
{
    public class MailController : BaseController
    {
        IMailoutManage MailoutManage;
        IMailinManage MailinManage;
        IContentManage ContentManage;
        IDepartmentManage DepartmentManage;

        public MailController
            (
                IMailoutManage MailoutManage,
                IMailinManage MailinManage,
                IContentManage ContentManage,
                IDepartmentManage DepartmentManage
            )
        {
            this.MailoutManage = MailoutManage;
            this.MailinManage = MailinManage;
            this.ContentManage = ContentManage;
            this.DepartmentManage = DepartmentManage;
        }      

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "View")]
        public ActionResult Index()
        {
            int MailInbox = ClsDic.DicMailType["收件箱"];
            int DraftBox = ClsDic.DicMailType["草稿箱"];
            int MailUnRead = ClsDic.DicMailReadStatus["未读"];
            base.ViewData["MailOutBox"] = this.MailinManage.LoadListAll((MAIL_INBOX p) => p.Recipient.Contains(this.CurrentUser.LogName + this.EmailDomain) && p.ReadStatus == MailUnRead && p.MailType == MailInbox).Count;
            base.ViewData["DraftBox"] = this.MailoutManage.LoadAll((MAIL_OUTBOX p) => p.FK_UserId == this.CurrentUser.LogName && p.MailType == DraftBox).ToList<MAIL_OUTBOX>().Count;
            return base.View();
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "WriteMail")]
        public ActionResult WriteMail(int? id)
        {
            MAIL_OUTBOX entity = new MAIL_OUTBOX();
            if (id.HasValue && id > 0)
            {
                entity = this.MailoutManage.Get((MAIL_OUTBOX p) => (int?)p.ID == id);
                base.ViewData["MailContent"] = (this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "MAIL_OUTBOX") ?? new COM_CONTENT());
            }
            base.ViewData["ReplayUser"] = base.Request.QueryString["toUser"];
            base.ViewData["EmailContacts"] = this.EmailContacts();
            return base.View();
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "View")]
        public ActionResult ReadMail(int id, string box)
        {
            ActionResult result;
            try
            {
                MAIL_OUTBOX entity = this.MailoutManage.Get((MAIL_OUTBOX p) => p.ID == id);
                base.ViewData["MailContent"] = ((this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "MAIL_OUTBOX") == null) ? "" : this.ContentManage.Get((COM_CONTENT p) => p.FK_RELATIONID == entity.FK_RELATIONID && p.FK_TABLE == "MAIL_OUTBOX").CONTENT);
                base.ViewData["EmailDomain"] = base.EmailDomain;
                if (box == "in")
                {
                    MAIL_INBOX mAIL_INBOX = this.MailinManage.LoadAll((MAIL_INBOX p) => p.MAIL_OUTBOX.ID == entity.ID && p.Recipient == this.CurrentUser.LogName + this.EmailDomain).FirstOrDefault<MAIL_INBOX>();
                    if (mAIL_INBOX != null)
                    {
                        mAIL_INBOX.ReadStatus = ClsDic.DicMailReadStatus["已读"];
                        this.MailinManage.Update(mAIL_INBOX);
                    }
                }
                result = base.View(entity);
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "查看邮件：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [ValidateInput(false), UserAuthorize(ModuleAlias = "Mail", OperaAction = "WriteMail")]
        public ActionResult Send(MAIL_OUTBOX entity)
        {
            bool isEdit = false;
            string fK_RELATIONID = "";
            JsonHelper jsonHelper = new JsonHelper
            {
                Msg = "邮件发送成功",
                Status = "n"
            };
            try
            {
                if (entity != null)
                {
                    int num = (base.Request["ContentId"] == null) ? 0 : int.Parse(base.Request["ContentId"].ToString());
                    if (entity.ID <= 0)
                    {
                        fK_RELATIONID = Guid.NewGuid().ToString();
                        entity.FK_RELATIONID = fK_RELATIONID;
                        entity.FK_UserId = base.CurrentUser.LogName;
                        entity.SendStatus = ClsDic.DicMailSendStatus["已发送"];
                        entity.SaveDate = DateTime.Now;
                        entity.SendDate = DateTime.Now;
                        entity.MailType = ClsDic.DicMailType["发件箱"];
                    }
                    else
                    {
                        fK_RELATIONID = entity.FK_RELATIONID;
                        entity.SendStatus = ClsDic.DicMailSendStatus["已发送"];
                        entity.MailType = ClsDic.DicMailType["发件箱"];
                        entity.SendDate = DateTime.Now;
                        isEdit = true;
                    }
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            string text = base.Request.Form["attachment"];
                            if (!string.IsNullOrEmpty(text))
                            {
                                List<string> list = (from p in text.Trim(new char[]
                                {
                                    ','
                                }).Split(new string[]
                                {
                                    ","
                                }, StringSplitOptions.RemoveEmptyEntries)
                                                     select p).ToList<string>();
                                if (list != null && list.Count > 0)
                                {
                                    foreach (string current in list)
                                    {
                                        List<string> list2 = (from p in current.Trim(new char[]
                                        {
                                            ';'
                                        }).Split(new string[]
                                        {
                                            ";"
                                        }, StringSplitOptions.RemoveEmptyEntries)
                                                              select p).ToList<string>();
                                        entity.MAIL_ATTACHMENT.Add(new MAIL_ATTACHMENT
                                        {
                                            AttName = list2[0],
                                            AttNewName = list2[1],
                                            AttPath = list2[2],
                                            AttExt = list2[3],
                                            AttSize = list2[4],
                                            CreateIP = Utils.GetIP(),
                                            UploadDate = DateTime.Now
                                        });
                                    }
                                }
                            }
                            entity.ToUser = entity.ToUser.ToLower();
                            List<string> list3 = (from p in entity.ToUser.Trim(new char[]
                            {
                                ','
                            }).Split(new string[]
                            {
                                ","
                            }, StringSplitOptions.RemoveEmptyEntries)
                                                  select p).ToList<string>();
                            foreach (string current2 in list3)
                            {
                                entity.MAIL_INBOX.Add(new MAIL_INBOX
                                {
                                    Recipient = current2,
                                    MailType = ClsDic.DicMailType["收件箱"],
                                    ReadStatus = ClsDic.DicMailReadStatus["未读"],
                                    ReceivingTime = DateTime.Now
                                });
                            }
                            if (this.MailoutManage.SaveOrUpdate(entity, isEdit))
                            {
                                if (num <= 0)
                                {
                                    this.ContentManage.Save(new COM_CONTENT
                                    {
                                        CONTENT = base.Request["Content"],
                                        FK_RELATIONID = fK_RELATIONID,
                                        FK_TABLE = "MAIL_OUTBOX",
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
                                        FK_TABLE = "MAIL_OUTBOX",
                                        CREATEDATE = DateTime.Now
                                    });
                                }
                                jsonHelper.ReUrl = "/Mail/Mail/Outbox";
                                jsonHelper.Status = "y";
                            }
                            else
                            {
                                jsonHelper.Msg = "邮件发送失败！";
                            }
                            transactionScope.Complete();
                        }
                        catch (Exception e)
                        {
                            jsonHelper.Msg = "邮件发送发生内部错误！";
                            base.WriteLog(enumOperator.None, "邮件发送失败：", e);
                        }
                        goto IL_4BA;
                    }
                }
                jsonHelper.Msg = "未找到要发送的邮件";
                IL_4BA:
                base.WriteLog(enumOperator.Edit, "发送邮件[" + entity.MailTitle + "]，结果：" + jsonHelper.Msg, enumLog4net.INFO);
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "邮件发送发生内部错误！";
                base.WriteLog(enumOperator.None, "邮件发送发生内部错误：", e2);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "MailOutbox")]
        public ActionResult Outbox()
        {
            ActionResult result;
            try
            {
               
                //MailController.< Outbox > o__SiteContainer15.<> p__Site16.Target(MailController.< Outbox > o__SiteContainer15.<> p__Site16, base.ViewBag, base.keywords);
                result = base.View(this.BindOutBoxList());
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "加载发件箱：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "Remove")]
        public ActionResult RemoveOutbox(string idList)
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Status = "n",
                Msg = "删除邮件成功"
            };
            try
            {
                if (string.IsNullOrEmpty(idList))
                {
                    jsonHelper.Msg = "未找到要删除的邮件";
                    return base.Json(jsonHelper);
                }
                string str = idList.TrimEnd(new char[]
                {
                    ','
                });
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("MailType", ClsDic.DicMailType["已删除"]);
                    this.MailoutManage.Modify("MAIL_OUTBOX", dictionary, "and ID in (" + str + ")");
                    base.WriteLog(enumOperator.Remove, "删除邮件：" + jsonHelper.Msg, enumLog4net.WARN);
                    jsonHelper.Status = "y";
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "删除邮件发生内部错误！";
                    base.WriteLog(enumOperator.Remove, "删除邮件：", e);
                }
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "删除邮件发生内部错误！";
                base.WriteLog(enumOperator.Remove, "删除邮件：", e2);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "Trash")]
        public ActionResult TrashInbox(string idList)
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Status = "n",
                Msg = "删除邮件成功"
            };
            try
            {
                if (string.IsNullOrEmpty(idList))
                {
                    jsonHelper.Msg = "未找到要删除的邮件";
                    return base.Json(jsonHelper);
                }
                string str = idList.TrimEnd(new char[]
                {
                    ','
                });
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("MailType", ClsDic.DicMailType["垃圾箱"]);
                    this.MailoutManage.Modify("MAIL_INBOX", dictionary, "and ID in (" + str + ")");
                    base.WriteLog(enumOperator.Remove, "删除邮件：" + jsonHelper.Msg, enumLog4net.WARN);
                    jsonHelper.Status = "y";
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "删除邮件发生内部错误！";
                    base.WriteLog(enumOperator.Remove, "删除邮件：", e);
                }
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "删除邮件发生内部错误！";
                base.WriteLog(enumOperator.Remove, "删除邮件：", e2);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "MailTrashbox")]
        public ActionResult Trashbox()
        {
            ActionResult result;
            try
            {
                
                //MailController.< Trashbox > o__SiteContainer19.<> p__Site1a.Target(MailController.< Trashbox > o__SiteContainer19.<> p__Site1a, base.ViewBag, base.keywords);
                result = base.View(this.BindTrashBoxList());
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "加载垃圾箱：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "Clear")]
        public ActionResult Delete(string idList)
        {
            JsonHelper jsonHelper = new JsonHelper
            {
                Status = "n",
                Msg = "删除邮件成功"
            };
            try
            {
                if (string.IsNullOrEmpty(idList))
                {
                    jsonHelper.Msg = "未找到要删除的邮件";
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
                try
                {
                    this.MailinManage.Delete((MAIL_INBOX p) => idList1.Contains(p.ID));
                    base.WriteLog(enumOperator.Remove, "删除邮件：" + jsonHelper.Msg, enumLog4net.WARN);
                    jsonHelper.Status = "y";
                }
                catch (Exception e)
                {
                    jsonHelper.Msg = "删除邮件发生内部错误！";
                    base.WriteLog(enumOperator.Remove, "删除邮件：", e);
                }
            }
            catch (Exception e2)
            {
                jsonHelper.Msg = "删除邮件发生内部错误！";
                base.WriteLog(enumOperator.Remove, "删除邮件：", e2);
            }
            return base.Json(jsonHelper);
        }

        [UserAuthorize(ModuleAlias = "Mail", OperaAction = "MailInbox")]
        public ActionResult Inbox()
        {
            ActionResult result;
            try
            {
                int MailInbox = ClsDic.DicMailType["收件箱"];
                int MailUnRead = ClsDic.DicMailReadStatus["未读"];
                base.ViewData["MailInBox"] = this.MailinManage.LoadListAll((MAIL_INBOX p) => p.Recipient.Contains(this.CurrentUser.LogName + this.EmailDomain) && p.ReadStatus == MailUnRead && p.MailType == MailInbox).Count;
               
                //MailController.< Inbox > o__SiteContainer20.<> p__Site21.Target(MailController.< Inbox > o__SiteContainer20.<> p__Site21, base.ViewBag, base.keywords);
                result = base.View(this.BindInBoxList());
            }
            catch (Exception ex)
            {
                base.WriteLog(enumOperator.Select, "加载收件箱：", ex);
                throw ex.InnerException;
            }
            return result;
        }

        private PageInfo BindOutBoxList()
        {
            int MailType = ClsDic.DicMailType["发件箱"];
            IQueryable<MAIL_OUTBOX> queryable = this.MailoutManage.LoadAll((MAIL_OUTBOX p) => p.FK_UserId == this.CurrentUser.LogName && p.MailType == MailType);
            queryable = from p in queryable
                        orderby p.SendDate descending
                        select p;
            PageInfo<MAIL_OUTBOX> pageInfo = this.MailoutManage.Query(queryable, base.pageindex, base.pagesize);
            var list = (from p in pageInfo.List
                        select new
                        {
                            ID = p.ID,
                            MailTitle = p.MailTitle,
                            ToUser = p.ToUser,
                            ToUsers = this.GetToUser(p.ToUser),
                            IsAttachement = p.MAIL_ATTACHMENT != null && p.MAIL_ATTACHMENT.Count > 0,
                            SendDate = (p.SendDate.Year < DateTime.Now.Year) ? p.SendDate.ToString("yyy年MM月dd日") : p.SendDate.ToString("MM月dd日 HH:mm:ss")
                        }).ToList();
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                list = (from p in list
                        where p.ToUser.Contains(base.keywords) || p.MailTitle.Contains(base.keywords)
                        select p).ToList();
            }
           //
           //
            return new PageInfo( pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(list));
        }

        private PageInfo BindInBoxList()
        {
            int MailType = ClsDic.DicMailType["收件箱"];
            IQueryable<MAIL_INBOX> queryable = this.MailinManage.LoadAll((MAIL_INBOX p) => p.Recipient.Contains(this.CurrentUser.LogName + this.EmailDomain) && p.MailType == MailType);
            queryable = from p in queryable
                        orderby p.ReceivingTime descending
                        select p;
            PageInfo<MAIL_INBOX> pageInfo = this.MailinManage.Query(queryable, base.pageindex, base.pagesize);
            var list = pageInfo.List.Select(delegate (MAIL_INBOX p)
            {
                return new
                {
                    ID = p.ID,
                    MailID = p.MAIL_OUTBOX.ID,
                    MailTitle = p.MAIL_OUTBOX.MailTitle,
                    ReadStatus = p.ReadStatus,
                    FromUser = (this.UserManage.Get((SYS_USER m) => m.ACCOUNT == p.MAIL_OUTBOX.FK_UserId) == null) ? "" : string.Concat(new string[]
                    {
                        "<span data-toggle=\"tooltip\" data-placement=\"top\" title=\"",
                        p.MAIL_OUTBOX.FK_UserId,
                        base.EmailDomain,
                        "\">",
                        this.UserManage.Get((SYS_USER m) => m.ACCOUNT == p.MAIL_OUTBOX.FK_UserId).NAME,
                        "</span>"
                    }),
                    IsAttachement = p.MAIL_OUTBOX.MAIL_ATTACHMENT != null && p.MAIL_OUTBOX.MAIL_ATTACHMENT.Count > 0,
                    ReceivingTime = (p.ReceivingTime.Year < DateTime.Now.Year) ? p.ReceivingTime.ToString("yyy年MM月dd日") : p.ReceivingTime.ToString("MM月dd日 HH:mm:ss")
                };
            }).ToList();
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                list = (from p in list
                        where p.FromUser.Contains(base.keywords) || p.MailTitle.Contains(base.keywords)
                        select p).ToList();
            }

            return new PageInfo(pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(list));
        }

        private PageInfo BindTrashBoxList()
        {
            int MailType = ClsDic.DicMailType["垃圾箱"];
            IQueryable<MAIL_INBOX> queryable = this.MailinManage.LoadAll((MAIL_INBOX p) => p.Recipient.Contains(this.CurrentUser.LogName + this.EmailDomain) && p.MailType == MailType);
            queryable = from p in queryable
                        orderby p.ReceivingTime descending
                        select p;
            PageInfo<MAIL_INBOX> pageInfo = this.MailinManage.Query(queryable, base.pageindex, base.pagesize);
            var list = pageInfo.List.Select(delegate (MAIL_INBOX p)
            {
                return new
                {
                    ID = p.ID,
                    MailID = p.MAIL_OUTBOX.ID,
                    MailTitle = p.MAIL_OUTBOX.MailTitle,
                    ReadStatus = p.ReadStatus,
                    FromUser = (this.UserManage.Get((SYS_USER m) => m.ACCOUNT == p.MAIL_OUTBOX.FK_UserId) == null) ? "" : string.Concat(new string[]
                    {
                        "<span data-toggle=\"tooltip\" data-placement=\"top\" title=\"",
                        p.MAIL_OUTBOX.FK_UserId,
                        base.EmailDomain,
                        "\">",
                        this.UserManage.Get((SYS_USER m) => m.ACCOUNT == p.MAIL_OUTBOX.FK_UserId).NAME,
                        "</span>"
                    }),
                    IsAttachement = p.MAIL_OUTBOX.MAIL_ATTACHMENT != null && p.MAIL_OUTBOX.MAIL_ATTACHMENT.Count > 0,
                    ReceivingTime = (p.ReceivingTime.Year < DateTime.Now.Year) ? p.ReceivingTime.ToString("yyy年MM月dd日") : p.ReceivingTime.ToString("MM月dd日 HH:mm:ss")
                };
            }).ToList();
            if (!string.IsNullOrEmpty(base.keywords))
            {
                base.keywords = base.keywords.ToLower();
                list = (from p in list
                        where p.FromUser.Contains(base.keywords) || p.MailTitle.Contains(base.keywords)
                        select p).ToList();
            }

            return new PageInfo(pageInfo.Index, pageInfo.PageSize, pageInfo.Count, JsonConverter.JsonClass(list));
        }

        private string GetToUser(string toUser)
        {
            string text = string.Empty;
            List<string> list = (from p in toUser.Trim(new char[]
            {
                ','
            }).Split(new string[]
            {
                ","
            }, StringSplitOptions.RemoveEmptyEntries)
                                 select p).ToList<string>();
            if (list != null && list.Count > 0)
            {
                using (List<string>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        //MailController.<> c__DisplayClass3f <> c__DisplayClass3f = new MailController.<> c__DisplayClass3f();

                        //<> c__DisplayClass3f.user = enumerator.Current;
                        //int subIndex = <> c__DisplayClass3f.user.IndexOf('@');
                        //SYS_USER sYS_USER = this.UserManage.Get((SYS_USER p) => p.ACCOUNT.ToLower() == <> c__DisplayClass3f.user.Substring(0, subIndex).ToLower());
                        SYS_USER sYS_USER = null;
                        if (sYS_USER != null)
                        {
                            string text2 = text;
                            text = string.Concat(new string[]
                            {
                                text2,
                                "<span data-toggle=\"tooltip\" data-placement=\"top\" title=\"",
                                sYS_USER.ACCOUNT,
                                base.EmailDomain,
                                "\">",
                                sYS_USER.NAME,
                                "</span>,"
                            });
                        }
                    }
                }
            }
            if (!text.EndsWith(","))
            {
                return text;
            }
            return text.TrimEnd(new char[]
            {
                ','
            });
        }

        private object EmailContacts()
        {
            var obj = from m in (from m in this.DepartmentManage.LoadAll((SYS_DEPARTMENT m) => m.BUSINESSLEVEL == (int?)1)
                                 orderby m.SHOWORDER
                                 select m).ToList<SYS_DEPARTMENT>()
                      select new
                      {
                          ID = m.ID,
                          DepartName = m.NAME,
                          UserList = this.GetDepartUsers(m.ID)
                      };
            return JsonConverter.JsonClass(obj);
        }

        private object GetDepartUsers(string departId)
        {
            List<string> departs = (from p in this.DepartmentManage.LoadAll((SYS_DEPARTMENT p) => p.PARENTID == departId)
                                    orderby p.SHOWORDER
                                    select p.ID).ToList<string>();
            return (from p in this.UserManage.LoadListAll((SYS_USER p) => p.ID != this.CurrentUser.Id && departs.Any((string e) => e == p.DPTID))
                    orderby p.LEVELS
                    orderby p.CREATEDATE
                    select new
                    {
                        ID = p.ID,
                        FaceImg = string.IsNullOrEmpty(p.FACE_IMG) ? ("/Pro/Project/User_Default_Avatat?name=" + p.NAME.Substring(0, 1)) : p.FACE_IMG,
                        NAME = p.NAME,
                        InsideEmail = p.ACCOUNT + base.EmailDomain,
                        LEVELS = p.LEVELS
                    }).ToList();
        }
    }
}
