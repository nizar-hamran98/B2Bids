using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SharedKernel;
public interface IRepositoryBase<T>
{
    DbContext Context { get; }
    Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task AddOrUpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    T Add(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity, CancellationToken cancellationToken = default);
    IEnumerable<T> AddRange(IEnumerable<T> entity);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    bool Delete(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void DeleteRange(IEnumerable<T> entities);
    Task<T> SoftDelete(T entity, CancellationToken cancellationToken = default);
    Task SoftDeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    IQueryable<T> GetAll();

    Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);
    Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
    int Count();

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    bool Any(Expression<Func<T, bool>> expression);

    IQueryable<T> Search(string FindMember, eFilterOperator SearchOperator, object? WithValue);
    IQueryable<T> Search(string FindMember, object? WithValueEquals);
    IQueryable<T> Search(SearchCondition SearchCondition);
    IQueryable<T> Search(SearchOptions SearchOptions, int PageSize = 0, int PageIndex = 0);
    IQueryable<T> Search(SearchQuery SearchQuery, IEnumerable<OrderByOptions> OrderByList = null, int PageSize = 0, int PageIndex = 0);

    IQueryable<T> ExecuteFunction(string functionName);
    IQueryable<T> ExecuteFunction(string functionName, string[] parameters);
}