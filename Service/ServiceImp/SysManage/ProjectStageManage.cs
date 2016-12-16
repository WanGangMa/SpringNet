using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class ProjectStageManage : RepositoryBase<PRO_PROJECT_STAGES>, IProjectStageManage, IRepository<PRO_PROJECT_STAGES>
    {
    }
}
