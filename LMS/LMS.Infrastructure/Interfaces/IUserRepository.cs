using LMS.Domain.Entities.Users;
using LMS.Infrastructure.DbContexts;
using LMS.Infrastructure.Repositories;

namespace LMS.Infrastructure.Interfaces
{
    public abstract class IUserRepository : BaseRepository<User>
    {
        private readonly AppDbContext _context;

        protected IUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.Users.Update(user);
                await SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        public async Task LockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is not null)
            {
                user.IsLocked = true;
                _context.Users.Update(user);
                await SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }
        }

    }
}
