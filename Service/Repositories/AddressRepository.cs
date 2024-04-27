using EllieApi.Models;
using EllieApi.Service.Interfaces;

namespace EllieApi.Service.Repositories
{
    public class AddressRepository : GenericRepository<Address, ElliedbContext>, IAddressRepository
    {
        public AddressRepository(ElliedbContext context)
            : base(context)
        {
        }
    }
}