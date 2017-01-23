using System.Collections.Generic;
using Domain;

namespace Service.IService
{
    public interface ISystemManage : IRepository<SYS_SYSTEM>, IAutofac
    {
        dynamic LoadSystemInfo(List<string> systems);
    }
}
