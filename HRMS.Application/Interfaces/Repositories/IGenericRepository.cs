using System.Linq.Expressions;

namespace HRMS.Application.Common.Interfaces;

/// <summary>
/// Generic repository interface for basic CRUD and query operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Retrieves an entity by its primary key.
    /// </summary>
    Task<T?> GetByIdAsync(object id);

    /// <summary>
    /// Retrieves an entity by its primary key with optional related entities included.
    /// </summary>
    Task<T?> GetByIdWithIncludesAsync(object id, params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Retrieves all entities of type T.
    /// </summary>
    Task<IReadOnlyList<T>> GetAllAsync();

    /// <summary>
    /// Adds a new entity to the database.
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity in the database.
    /// </summary>
    Task Update(T entity);

    /// <summary>
    /// Deletes an existing entity from the database.
    /// </summary>
    Task Delete(T entity);

    /// <summary>
    /// Checks whether any entity matches the given predicate.
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Finds the first entity that matches the given predicate.
    /// </summary>
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves all entities matching the given predicate.
    /// </summary>
    Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate);
}