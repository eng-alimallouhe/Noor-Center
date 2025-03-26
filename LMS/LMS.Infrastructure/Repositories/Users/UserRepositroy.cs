using LMS.Infrastructure.DbContexts;
using LMS.Infrastructure.Interfaces;

namespace LMS.Infrastructure.Repositories.Users
{
    public class UserRepositroy : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepositroy(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
