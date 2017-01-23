using Domain;
using Service.IService;

namespace Service.ServiceImp
{
    public class MailinManage : RepositoryBase<MAIL_INBOX>, IMailinManage, IRepository<MAIL_INBOX>
    {
    }
}
