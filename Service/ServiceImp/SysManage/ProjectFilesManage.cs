using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class ProjectFilesManage : RepositoryBase<PRO_PROJECT_FILES>, IProjectFilesManage, IRepository<PRO_PROJECT_FILES>
    {
    }
}
