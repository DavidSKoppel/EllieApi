using EllieApi.Models;
using EllieApi.Service.Interfaces;
using EllieApi.Service.Repositories;

namespace EllieApi.Service.Repositories
{
    public class AlarmTypeRepository : GenericRepository<AlarmType, ElliedbContext>, IAlarmTypeRepository
    {
        public AlarmTypeRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}