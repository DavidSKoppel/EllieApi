using EllieApi.Models;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class InstituteRepository : GenericRepository<Institute, ElliedbContext>, IInstituteRepository
    {
        public InstituteRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}