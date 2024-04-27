using EllieApi.Models;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class AlarmRepository : GenericRepository<Alarm, ElliedbContext>, IAlarmRepository
    {
        public AlarmRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}