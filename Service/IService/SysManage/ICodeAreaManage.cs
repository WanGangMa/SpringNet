//using System;
using System.Linq;
using Domain;

namespace Service.IService
{
    public interface ICodeAreaManage : IRepository<SYS_CODE_AREA>, IAutofac
    {
        IQueryable<SYS_CODE_AREA> LoadProvince();

        IQueryable<SYS_CODE_AREA> LoadCity(string provinceId);

        IQueryable<SYS_CODE_AREA> LoadCountry(string cityId);

        IQueryable<SYS_CODE_AREA> LoadCommunity(string countryId);
    }
}
