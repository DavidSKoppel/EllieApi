using EllieApi.Models;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee, ElliedbContext>, IEmployeeRepository
    {
        public EmployeeRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}