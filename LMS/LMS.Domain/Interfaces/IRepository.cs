using System.Linq.Expressions;

namespace LMS.Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<ICollection<TEntity>> GettAllAsync();

        Task<TEntity?> GetByIdAsync(int id);

        Task<TEntity> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task<ICollection<TEntity>> GetAllByExpressionAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity?> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression);
    }
}
