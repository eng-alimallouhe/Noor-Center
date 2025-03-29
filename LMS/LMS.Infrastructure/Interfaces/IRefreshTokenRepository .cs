using LMS.Domain.Entities.Users;

namespace LMS.Infrastructure.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByUserIdAsync(int userId);
        Task AddAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
        Task DeleteAsync(int userId);
    }
}
