using System.Linq.Expressions;
using HRMS.Application.Common.Interfaces;
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
    public virtual async Task<T?> GetByIdAsync(object id)
    {
        return await dbContext.Set<T>()
            .AsNoTracking()
            .SingleOrDefaultAsync(e => EF.Property<object>(e, "Id") == id);
    }

    /// <summary>
    /// Retrieves an entity by its primary key with optional relationships included.
    /// </summary>
    public virtual async Task<T?> GetByIdWithIncludesAsync(object id, params Expression<Func<T, object>>[] includes)
    {
        var query = dbContext.Set<T>().AsQueryable().AsNoTracking();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        var entityType = dbContext.Model.FindEntityType(typeof(T));
        var primaryKey = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();

        if (primaryKey == null)
            throw new InvalidOperationException($"Entity {typeof(T).Name} does not have a primary key defined.");

        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, primaryKey.Name);
        var equals = Expression.Equal(property, Expression.Constant(id));
        var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

        return await query.FirstOrDefaultAsync(lambda);
    }

    /// <summary>
    /// Adds a new entity to the database asynchronously.
    /// </summary>
    public async Task<T> AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Marks an entity as modified in the DbContext.
    /// </summary>
    public Task Update(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes an entity from the DbContext.
    /// </summary>
    public Task Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves all entities of type T from the database.
    /// </summary>
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await dbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Returns true if any entity matches the predicate.
    /// </summary>
    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return dbContext.Set<T>().AnyAsync(predicate);
    }

    /// <summary>
    /// Finds the first entity matching the predicate.
    /// </summary>
    public Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// Gets a list of entities matching the predicate.
    /// </summary>
    public Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate)
    {
        return dbContext.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
    }
}
