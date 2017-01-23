using Domain;

namespace Service.IService
{
    public interface IMailoutManage : IRepository<MAIL_OUTBOX>, IAutofac
    {
    }
}
