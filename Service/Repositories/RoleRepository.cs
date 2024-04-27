using EllieApi.Models;
using EllieApi.Service.Repositories;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class RoleRepository : GenericRepository<Role, ElliedbContext>, IRoleRepository
    {
        public RoleRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}