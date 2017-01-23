using Common;
using Domain;
using Service.IService;
using System.Collections.Generic;
using System.Linq;

namespace Service.ServiceImp
{
    public class SystemManage : RepositoryBase<SYS_SYSTEM>, ISystemManage, IRepository<SYS_SYSTEM>
    {
        public dynamic LoadSystemInfo(List<string> systems)
        {
            return JsonConverter.JsonClass((from p in this.LoadAll((SYS_SYSTEM p) => systems.Any((string e) => e == p.ID))
                                            orderby p.CREATEDATE
                                            select new
                                            {
                                                p.ID,
                                                p.NAME
                                            }).ToList());
        }
    }
}
