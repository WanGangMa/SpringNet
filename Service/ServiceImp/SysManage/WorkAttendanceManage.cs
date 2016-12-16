using Domain;
using Service.IService;
using System;

namespace Service.ServiceImp
{
    public class WorkAttendanceManage : RepositoryBase<COM_WORKATTENDANCE>, IWorkAttendanceManage, IRepository<COM_WORKATTENDANCE>
    {
    }
}
