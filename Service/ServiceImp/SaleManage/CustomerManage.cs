using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.IService;

namespace Service.ServiceImp
{
    class CustomerManage:RepositoryBase<Domain.t_Customers>,ICustomerManage
    {
    }
}
