using System.Linq.Expressions;

namespace PM.Common.Interfaces
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity> CreateAndGetAsync(TEntity entity);

        int Count();
        Task<int> CountAsync();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        Task AddManyAsync(IEnumerable<TEntity> entities);
        Task<List<TEntity>> AddManyAndGetAsync(IEnumerable<TEntity> entities);

        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        // 🔹 Get single entity by ID
        Task<TEntity> GetByIdAsync(TPrimaryKey id);

        // 🔹 Get all entities
        Task<IEnumerable<TEntity>> GetAllAsync();

        // 🔹 Get entity with a condition (supports eager loading)
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                           CancellationToken cancellationToken = default,
                                           params Expression<Func<TEntity, object>>[] includes);

        // 🔹 Get filtered list
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

        // 🔹 Add new entity
        Task AddAsync(TEntity entity);

        // 🔹 Update existing entity
        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);

        // 🔹 Delete by entity
        void Delete(TEntity entity);

        Task DeleteAsync(TEntity entity);

        // 🔹 Delete by ID
        Task DeleteByIdAsync(TPrimaryKey id);

        // 🔹 Save changes (Unit of Work)
        Task<int> SaveChangesAsync();


        // 🔹 Execute raw SQL command
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        // 🔹 Include relationships for eager loading
        IQueryable<TEntity> GetIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
