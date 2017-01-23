using Domain;
using Service.IService;

namespace Service.ServiceImp
{
    public class MailAttachmentManage : RepositoryBase<MAIL_ATTACHMENT>, IMailAttachmentManage, IRepository<MAIL_ATTACHMENT>
    {
    }
}
