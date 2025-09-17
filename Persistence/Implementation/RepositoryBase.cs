using Kernel.Contract;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Linq.Expressions;
using System.Reflection;

namespace Persistence;
public class RepositoryBase<T> : IRepositoryBase<T>
       where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;
    private readonly IHttpContext _httpContext;
    public DbContext Context { get; private set; }

    public RepositoryBase(DbContext context, IHttpContext httpContext)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = Context.Set<T>();
        _httpContext = httpContext;
    }

    #region Add
    public async Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default))
            await AddAsync(entity, cancellationToken);
        else
            await UpdateAsync(entity, cancellationToken);
    }

    public async Task AddOrUpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default)
    {
        if (entity.Id.Equals(default))
            await AddAsync(entity, cancellationToken);
        else
            await UpdateAsync(entity,Changes, cancellationToken);
    }
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTimeOffset.UtcNow;
        if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.CreatedBy = _httpContext.IntranetUser?.UserName;

        await _dbSet.AddAsync(entity);

        return entity;
    }
    public T Add(T entity) => AddAsync(entity).ConfigureAwait(false).GetAwaiter().GetResult();
    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entity);
        return entity;
    }
    public IEnumerable<T> AddRange(IEnumerable<T> entity) => AddRangeAsync(entity).ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Update
    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.UpdatedBy = _httpContext.IntranetUser?.UserName;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        _dbSet.Entry(entity).State = EntityState.Modified;
        _dbSet.Update(entity);

        await Task.CompletedTask;

        return entity;
    }
    public async Task<T> UpdateAsync(T entity, string Changes, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.UpdatedBy = _httpContext.IntranetUser?.UserName;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        _dbSet.Entry(entity).State = EntityState.Modified;
        _dbSet.Update(entity);

        await Task.CompletedTask;

        return entity;
    }
    public void Update(T entity) => UpdateAsync(entity).ConfigureAwait(false).GetAwaiter().GetResult();
    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (!string.IsNullOrWhiteSpace(_httpContext.IntranetUser?.UserName)) entity.UpdatedBy = _httpContext.IntranetUser?.UserName;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            _dbSet.Entry(entity).State = EntityState.Modified;
        }

        _dbSet.UpdateRange(entities);

        return Task.CompletedTask;
    }
    public void UpdateRange(IEnumerable<T> entities) => UpdateRangeAsync(entities).ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Delete
    public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Entry(entity).State = EntityState.Deleted;
        var result = _dbSet.Remove(entity);
        return await Task.FromResult(result.State == EntityState.Deleted);
    }
    public bool Delete(T entity) => DeleteAsync(entity, default).ConfigureAwait(false).GetAwaiter().GetResult();
    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var item in entities)
            _dbSet.Entry(item).State = EntityState.Deleted;

        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }
    public void DeleteRange(IEnumerable<T> entities) => DeleteRangeAsync(entities).ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<T> SoftDelete(T entity, CancellationToken cancellationToken = default)
    {
        entity.StatusId = (short)EntityStatus.Deleted;
        _dbSet.Entry(entity).State = EntityState.Deleted;
        _dbSet.Update(entity);
        await Task.CompletedTask;
        return entity;
    }
    public Task SoftDeleteRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
            entity.StatusId = (short)EntityStatus.Deleted;
        return Task.CompletedTask;
    }
    #endregion

    #region Get
    public IQueryable<T> GetAll() => Context.Set<T>();
    #endregion

    #region Select
    public async Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) => await query.ToListAsync();
    public async Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) => await query.FirstOrDefaultAsync();
    #endregion

    #region Count
    public async Task<int> CountAsync(CancellationToken cancellationToken = default) => await _dbSet.CountAsync();
    public int Count() => CountAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Any
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default) => await _dbSet.AnyAsync(expression);
    public bool Any(Expression<Func<T, bool>> expression) => AnyAsync(expression).ConfigureAwait(false).GetAwaiter().GetResult();
    #endregion

    #region Excute Functions
    public IQueryable<T> ExecuteFunction(string functionName)
    {
        return _dbSet.FromSql($"{functionName}").AsNoTracking();
    }

    public IQueryable<T> ExecuteFunction(string functionName, string[] parameters)
    {
        var formattedQuery = string.Format(functionName, parameters);
        return _dbSet.FromSql($"{functionName}").AsNoTracking();
    }
    #endregion

    #region Search
    public IQueryable<T> Search(string FindMember, eFilterOperator SearchOperator, object? WithValue)
    {
        var target = GetAll();
        return target.Search(new SearchQuery(new FilterByOptions(FindMember, SearchOperator, WithValue)));
    }

    public IQueryable<T> Search(string FindMember, object? WithValueEquals)
    {
        var target = GetAll();
        return target.Search(new SearchQuery(new FilterByOptions(FindMember, eFilterOperator.EqualsTo, WithValueEquals)));
    }

    public IQueryable<T> Search(SearchCondition SearchCondition)
    {
        var target = GetAll();
        return target.Search(new SearchQuery(SearchCondition));
    }

    public IQueryable<T> Search(SearchOptions SearchOptions, int PageSize = 0, int PageIndex = 0)
    {
        var target = GetAll();
        SearchQuery searchQuery = new SearchQuery();
        if (SearchOptions.SearchMatchType == eSearchMatchType.MatchAll)
        {
            SearchCondition searchCondition = new SearchCondition();
            searchQuery.Conditions.Add(new SearchCondition
            {
                Criteria = new List<FilterByOptions>(SearchOptions.FilterOptions)
            });
        }
        else
        {
            foreach (FilterByOptions filterOption in SearchOptions.FilterOptions)
            {
                List<FilterByOptions> list = new List<FilterByOptions>();
                list.Add(filterOption);
                searchQuery.Conditions.Add(new SearchCondition
                {
                    Criteria = list
                });
            }
        }

        return target.Search(searchQuery, SearchOptions.OrderOptions, PageSize, PageIndex);
    }

    public IQueryable<T> Search(SearchQuery SearchQuery, IEnumerable<OrderByOptions> OrderByList = null, int PageSize = 0, int PageIndex = 0)
    {
        var Target = GetAll();
        var parameterExpression = Expression.Parameter(typeof(T), "TEntity");
        var expression = SearchExtensions.BuildSearchExpression<T>(parameterExpression, SearchQuery.Conditions);
        if (expression != null)
        {
            var predicate = Expression.Lambda<Func<T, bool>>(expression, new ParameterExpression[1] { parameterExpression });
            Target = Target.Where(predicate);
        }

        if (OrderByList != null && OrderByList.Any())
        {
            var orderByOptions = OrderByList.FirstOrDefault();
            if (orderByOptions != null && orderByOptions.MemberName.Contains("."))
            {
                var array = orderByOptions.MemberName.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries);
                Expression expression2 = parameterExpression;
                var type = typeof(T);
                foreach (var text in array)
                {
                    expression2 = Expression.Property(expression2, text);
                    type = type.GetProperty(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                }

                SearchExtensions.BuildOrderByThenBy(expression2, orderByOptions, type, ref Target, IsMainOrderBy: true);
            }
            else
            {
                if (orderByOptions != null)
                {
                    var propertyType = typeof(T).GetProperty(orderByOptions.MemberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                    SearchExtensions.BuildOrderByThenBy(null, orderByOptions, propertyType, ref Target, IsMainOrderBy: true);
                }
            }

            var list = new List<OrderByOptions> { orderByOptions };

            foreach (var item in OrderByList.Except(list.AsEnumerable()))
            {
                if (item.MemberName.Contains("."))
                {
                    var array3 = item.MemberName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    Expression expression3 = parameterExpression;
                    var type2 = typeof(T);
                    var array4 = array3;
                    foreach (var text2 in array4)
                    {
                        expression3 = Expression.Property(expression3, text2);
                        type2 = type2.GetProperty(text2, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                    }

                    SearchExtensions.BuildOrderByThenBy(expression3, item, type2, ref Target, IsMainOrderBy: false);
                }
                else
                {
                    var propertyType2 = typeof(T).GetProperty(item.MemberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                    SearchExtensions.BuildOrderByThenBy(null, item, propertyType2, ref Target, IsMainOrderBy: false);
                }
            }
        }
        else if (PageSize > 0 && PageIndex > 0)
        {
            var propertyInfo = typeof(T).GetProperty("ID", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo == null)
            {
                propertyInfo = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();
            }

            if (propertyInfo != null)
            {
                var propertyType3 = propertyInfo.PropertyType;
                var oO = new OrderByOptions
                {
                    MemberName = propertyInfo.Name,
                    SortOrder = OrderByOptions.Order.DEC
                };
                SearchExtensions.BuildOrderByThenBy(null, oO, propertyType3, ref Target, IsMainOrderBy: true);
            }
        }

        if (PageSize > 0 && PageIndex > 0)
        {
            Target = Target.Skip(PageIndex * PageSize).Take(PageSize);
        }
        else if (PageSize > 0)
        {
            Target = Target.Take(PageSize);
        }

        return Target;
    }
    #endregion

}

