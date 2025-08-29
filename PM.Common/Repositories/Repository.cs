using Microsoft.EntityFrameworkCore;
using PM.Common.Interfaces;
using PM.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PM.Common.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> CreateAndGetAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public int Count() => _dbSet.Count();
        public async Task<int> CountAsync() => await _dbSet.CountAsync();
        public int Count(Expression<Func<TEntity, bool>> predicate) => _dbSet.Count(predicate);
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.CountAsync(predicate);

        public IQueryable<TEntity> GetAll() => _dbSet;

        public async Task AddManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> AddManyAndGetAsync(IEnumerable<TEntity> entities)
        {
            var entityList = entities.ToList();

            await _dbSet.AddRangeAsync(entityList);
            await _context.SaveChangesAsync();

            return entityList;
        }

        // 🔹 Get entity by ID
        public async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        // 🔹 Get all entities
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                                        CancellationToken cancellationToken = default,
                                                        params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.FirstOrDefaultAsync(cancellationToken);

        }


        // 🔹 Get list of entities based on condition
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // 🔹 Add entity
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // 🔹 Update entity
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        // 🔹 Delete entity
        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        // 🔹 Delete entity by ID
        public async Task DeleteByIdAsync(TPrimaryKey id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        // 🔹 Save changes (Unit of Work)
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // 🔹 Check if any entity matches condition
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        // 🔹 Execute raw SQL command
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        // 🔹 Include relationships (eager loading)
        public IQueryable<TEntity> GetIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }
            return query;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _dbSet.Where(predicate).ToListAsync();
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
