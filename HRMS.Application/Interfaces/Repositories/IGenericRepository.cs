using System.Linq.Expressions;

namespace HRMS.Application.Interfaces.Repositories;

/// <summary>
/// Represents a generic repository interface for performing CRUD operations on entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Retrieves an entity by its ID, including related entities specified by the include expressions.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <returns>A task representing the asynchronous operation, returning the entity if found; otherwise, <c>null</c>.</returns>
    Task<T> GetByIdAsyncIncludeRelationship(object id, params Expression<Func<T, object>>[] includes);

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, returning a read-only list of entities.</returns>
    Task<IReadOnlyList<T>> GetAllAsync();

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation, returning the added entity.</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);
}