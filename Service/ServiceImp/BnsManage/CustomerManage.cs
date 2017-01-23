using Service.IService;

namespace Service.ServiceImp
{
    class CustomerManage:RepositoryBase<Domain.t_Customers>,ICustomerManage
    {
    }
}
