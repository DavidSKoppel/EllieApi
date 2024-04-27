using EllieApi.Models;
using EllieApi.Service.Repositories;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class UserRepository : GenericRepository<User, ElliedbContext>, IUserRepository
    {
        public UserRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}