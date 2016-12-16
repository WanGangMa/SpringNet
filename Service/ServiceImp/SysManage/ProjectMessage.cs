using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class ProjectMessage : RepositoryBase<PRO_PROJECT_MESSAGE>, IProjectMessage, IRepository<PRO_PROJECT_MESSAGE>
    {
    }
}
