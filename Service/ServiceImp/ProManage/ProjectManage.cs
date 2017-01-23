using Domain;
using Service.IService;

namespace Service.ServiceImp
{
    public class ProjectManage : RepositoryBase<PRO_PROJECTS>, IProjectManage, IRepository<PRO_PROJECTS>
    {
    }
}
