namespace LMS.Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<ICollection<TEntity>> GettAllAsync(ISpecification<TEntity> specification);
        Task<TEntity?> GetBySpecificationAsync(ISpecification<TEntity> specification);
        Task<TEntity?> GetByIdAsync(int id);

        Task<TEntity> AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);
    }
}
