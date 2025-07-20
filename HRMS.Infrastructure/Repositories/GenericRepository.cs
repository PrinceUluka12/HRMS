using System.Linq.Expressions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

/// <summary>
    /// A generic repository implementation for handling basic CRUD operations.
    /// </summary>
    public class GenericRepository<T>(DbContext dbContext) : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key value of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public virtual async Task<T> GetByIdAsyncIncludeRelationship(object id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }


        public virtual async Task<T> GetByIdAsyncIncludeRelationship(object id, params Expression<Func<T, object>>[] includes )
        {

            // Start with the base query
            var query = dbContext.Set<T>().AsQueryable();

            // Apply the include expressions
            // Include related data if specified
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            // Get the primary key property name
            var entityType = dbContext.Model.FindEntityType(typeof(T));
            var primaryKey = entityType.FindPrimaryKey();
            var primaryKeyProperty = primaryKey.Properties.FirstOrDefault();

            if (primaryKeyProperty == null)
            {
                throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a primary key defined.");
            }

            // Create an expression to compare the primary key
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, primaryKeyProperty.Name);
            var equals = Expression.Equal(property, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

            // Execute the query and find the entity by primary key
            return await query.FirstOrDefaultAsync(lambda);
        }

        /// <summary>
        /// Adds a new entity to the database asynchronously.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>The added entity.</returns>
        public async Task<T> AddAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Marks an entity as modified in the DbContext, so changes will be saved on the next commit.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public async void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public async void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
           
        }

        /// <summary>
        /// Retrieves all entities of type T from the database.
        /// </summary>
        /// <returns>A read-only list of all entities.</returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbContext
                 .Set<T>()
                 .AsNoTracking() // Improves performance by avoiding tracking for read-only operations.
                 .ToListAsync();
        }

        /// <summary>
        /// Paginates a given queryable dataset.
        /// </summary>
        /// <typeparam name="TEntity">The entity type being paginated.</typeparam>
        /// <param name="query">The queryable dataset.</param>
        /// <param name="pageNumber">The page number (starting from 1).</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>A paginated response containing the data and total record count.</returns>
        /// 
        /*protected async Task<PaginationResponseDto<TEntity>> Paged<TEntity>(IQueryable<TEntity> query, int pageNumber, int pageSize) where TEntity : class
        {
            var count = await query.CountAsync();

            var pagedResult = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new(pagedResult, count, pageNumber, pageSize);
        }*/
    }