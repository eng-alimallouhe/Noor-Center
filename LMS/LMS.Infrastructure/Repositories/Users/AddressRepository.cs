using LMS.Domain.Entities.Users;
using LMS.Infrastructure.DbContexts;

namespace LMS.Infrastructure.Repositories.Users
{

    public class NtofifcationRepository : BaseRepository<Address>
    {
        private readonly AppDbContext _context;
        public NtofifcationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task DeleteAsync(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await SaveChangesAsync();
            }
            else
            {
                throw new Exception("Address not found");
            }
        }
    }
}
