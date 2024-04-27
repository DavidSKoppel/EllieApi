using EllieApi.Models;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class NoteRepository : GenericRepository<Note, ElliedbContext>, INoteRepository
    {
        public NoteRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}