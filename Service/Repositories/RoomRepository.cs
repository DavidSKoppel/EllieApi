using EllieApi.Models;
using EllieApi.Service.Repositories;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class RoomRepository : GenericRepository<Room, ElliedbContext>, IRoomRepository
    {
        public RoomRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}