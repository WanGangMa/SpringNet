using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class MailAttachmentManage : RepositoryBase<MAIL_ATTACHMENT>, IMailAttachmentManage, IRepository<MAIL_ATTACHMENT>
    {
    }
}
