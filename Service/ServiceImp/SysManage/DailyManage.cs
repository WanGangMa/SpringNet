using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class DailyManage : RepositoryBase<COM_DAILYS>, IDailyManage, IRepository<COM_DAILYS>
    {
    }
}
