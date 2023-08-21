using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ECommerce.DAL.Models;

namespace ECommerce.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async virtual Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public void DetachEntity(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Unchanged;
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, bool>>[] where)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(x => x.IsDeleted == false);

            foreach (var item in where)
            {
                query = query.Where(item);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
        }


        public async Task<TEntity> GetByIdMultiLevelAsync(int id, params Tuple<Expression<Func<TEntity, object>>, Expression<Func<object, object>>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            foreach (var item in includes)
            {

                if (item.Item2 != null)
                {
                    query = query.Include(item.Item1).ThenInclude(item.Item2);
                }
                else
                {
                    query = query.Include(item.Item1);

                }
            }
            return await query.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
        }


        public void Remove(TEntity entity)
        {
            entity.LastUpdateTime = DateTime.UtcNow;
            entity.IsDeleted = true;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.LastUpdateTime = DateTime.UtcNow;
                entity.IsDeleted = true;
            }
        }
    }
}
