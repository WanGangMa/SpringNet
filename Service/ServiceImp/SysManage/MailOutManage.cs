using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class MailoutManage : RepositoryBase<MAIL_OUTBOX>, IMailoutManage, IRepository<MAIL_OUTBOX>
    {
    }
}
