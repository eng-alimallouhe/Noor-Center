using BrightCenter.Infrastructure.DbContexts;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LMS.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task<ICollection<TEntity>> GettAllAsync(ISpecification<TEntity> specification)
        {
            var query = SpecificationQueryBuilder.GetQuery(_dbSet, specification);
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetBySpecificationAsync(ISpecification<TEntity> specification)
        {
            var query = SpecificationQueryBuilder.GetQuery(_dbSet, specification);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public abstract Task DeleteAsync(int id);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
