using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class ProjectTeamManage : RepositoryBase<PRO_PROJECT_TEAMS>, IProjectTeamManage, IRepository<PRO_PROJECT_TEAMS>
    {
    }
}
