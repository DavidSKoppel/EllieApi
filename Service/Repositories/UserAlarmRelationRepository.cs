using EllieApi.Models;
using EllieApi.Service.Repositories;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class UserAlarmRelationRepository : GenericRepository<UserAlarmRelation, ElliedbContext>, IUserAlarmRelationRepository
    {
        public UserAlarmRelationRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}