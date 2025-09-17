using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel
{
    public static class SearchExtensions
    {
        public static IQueryable<T> Paged<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }

        //
        // Summary:
        //     Add capability to search through the dataset with string field name
        //
        // Parameters:
        //   Target:
        //     Queryable data set
        //
        //   FindMember:
        //     Searchable field string name
        //
        //   SearchOperator:
        //     Search Operator
        //
        //   WithValue:
        //     Value to compare
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> Target, string FindMember, eFilterOperator SearchOperator, object? WithValue)
        {
            return Target.Search(new SearchQuery(new FilterByOptions(FindMember, SearchOperator, WithValue)));
        }

        //
        // Summary:
        //     Add capability to search through the dataset with string field name
        //
        // Parameters:
        //   Target:
        //     Queryable data set
        //
        //   FindMember:
        //     Searchable field string name
        //
        //   WithValueEquals:
        //     Value to compare equals to
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> Target, string FindMember, object? WithValueEquals)
        {
            return Target.Search(new SearchQuery(new FilterByOptions(FindMember, eFilterOperator.EqualsTo, WithValueEquals)));
        }

        //
        // Summary:
        //     Add capability to search through the dataset with search condition
        //
        // Parameters:
        //   Target:
        //     Queryable data set
        //
        //   SearchCondition:
        //     search condition
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> Target, SearchCondition SearchCondition)
        {
            return Target.Search(new SearchQuery(SearchCondition));
        }

        //
        // Summary:
        //     Add capability to search through the dataset with search options
        //
        // Parameters:
        //   Target:
        //     Queryable data set
        //
        //   SearchOptions:
        //     search options
        //
        //   PageSize:
        //     Pagination results count limit
        //
        //   PageIndex:
        //     Pagination target page index 'Page number 2 => page index 1'
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> Target, SearchOptions SearchOptions, int PageSize = 0, int PageIndex = 0)
        {
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

            return Target.Search(searchQuery, SearchOptions.OrderOptions, PageSize, PageIndex);
        }

        //
        // Summary:
        //     Add capability to search through the dataset with search query
        //
        // Parameters:
        //   Target:
        //     Queryable data set
        //
        //   SearchQuery:
        //     search query
        //
        //   OrderByList:
        //     Sorting results with order by options
        //
        //   PageSize:
        //     Pagination results count limit
        //
        //   PageIndex:
        //     Pagination target page index 'Page number 2 => page index 1'
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> Target, SearchQuery SearchQuery, IEnumerable<OrderByOptions> OrderByList = null, int PageSize = 0, int PageIndex = 0)
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity), "TEntity");
            var expression = BuildSearchExpression<TEntity>(parameterExpression, SearchQuery.Conditions);
            if (expression != null)
            {
                var predicate = Expression.Lambda<Func<TEntity, bool>>(expression, new ParameterExpression[1] { parameterExpression });
                Target = Target.Where(predicate);
            }

            if (OrderByList != null && OrderByList.Any())
            {
                var orderByOptions = OrderByList.FirstOrDefault();
                if (orderByOptions != null && orderByOptions.MemberName.Contains("."))
                {
                    var array = orderByOptions.MemberName.Split(new string[1] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    Expression expression2 = parameterExpression;
                    var type = typeof(TEntity);
                    foreach (var text in array)
                    {
                        expression2 = Expression.Property(expression2, text);
                        type = type.GetProperty(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                    }

                    BuildOrderByThenBy(expression2, orderByOptions, type, ref Target, IsMainOrderBy: true);
                }
                else
                {
                    if (orderByOptions != null)
                    {
                        var propertyType = typeof(TEntity).GetProperty(orderByOptions.MemberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                        BuildOrderByThenBy(null, orderByOptions, propertyType, ref Target, IsMainOrderBy: true);
                    }
                }

                var list = new List<OrderByOptions> { orderByOptions };

                foreach (var item in OrderByList.Except(list.AsEnumerable()))
                {
                    if (item.MemberName.Contains("."))
                    {
                        var array3 = item.MemberName.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        Expression expression3 = parameterExpression;
                        var type2 = typeof(TEntity);
                        var array4 = array3;
                        foreach (var text2 in array4)
                        {
                            expression3 = Expression.Property(expression3, text2);
                            type2 = type2.GetProperty(text2, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                        }

                        BuildOrderByThenBy(expression3, item, type2, ref Target, IsMainOrderBy: false);
                    }
                    else
                    {
                        var propertyType2 = typeof(TEntity).GetProperty(item.MemberName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)!.PropertyType;
                        BuildOrderByThenBy(null, item, propertyType2, ref Target, IsMainOrderBy: false);
                    }
                }
            }
            else if (PageSize > 0 && PageIndex > 0)
            {
                var propertyInfo = typeof(TEntity).GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (propertyInfo == null)
                {
                    propertyInfo = typeof(TEntity).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();
                }

                if (propertyInfo != null)
                {
                    var propertyType3 = propertyInfo.PropertyType;
                    var oO = new OrderByOptions
                    {
                        MemberName = propertyInfo.Name,
                        SortOrder = OrderByOptions.Order.DEC
                    };
                    BuildOrderByThenBy(null, oO, propertyType3, ref Target, IsMainOrderBy: true);
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

        //
        // Summary:
        //     Add capability to search through the dataset with search query
        //
        // Parameters:
        //   Target:
        //     Queryable data set
        //
        //   NavigationProperty:
        //     Navigation property to be included in the results
        //
        //   SearchQuery:
        //     search query
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        //
        //   TEntityMany:
        //     Data set navigation property type
        public static IQueryable<TEntity> SearchMany<TEntity, TEntityMany>(this IQueryable<TEntity> Target, string NavigationProperty, SearchQuery SearchQuery)
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity), "TEntity");
            var parameterExpression2 = Expression.Parameter(typeof(TEntityMany), "TEntityMany");
            var expression = BuildSearchExpression<TEntityMany>(parameterExpression2, SearchQuery.Conditions);
            {
                var method = (from m in typeof(Enumerable).GetMethods()
                              where m.Name == "Any" & m.GetParameters().Length == 2
                              select m).Single().MakeGenericMethod(typeof(TEntityMany));
                var arg = Expression.Lambda<Func<TEntityMany, bool>>(expression, parameterExpression2);
                var predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Call(method, Expression.Property(parameterExpression, typeof(TEntity).GetProperty(NavigationProperty, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)), arg), new ParameterExpression[1] { parameterExpression });
                Target = Target.Where(predicate);
            }

            return Target;
        }

        //
        // Summary:
        //     Build search expression from search conditions, to convert strings
        //     to lamda expression
        //
        // Parameters:
        //   Parameter:
        //     Root expression
        //
        //   SearchConditions:
        //     search condition
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        public static Expression BuildSearchExpression<TEntity>(Expression Parameter, IEnumerable<SearchCondition> SearchConditions)
        {
            Expression expression = null;
            var num = 0;
            foreach (var searchCondition in SearchConditions)
            {
                Expression expression2 = null;
                var num2 = 0;
                foreach (var criterion in searchCondition.Criteria)
                {
                    var expression3 = DatabaseProviderConfiguration.SearchProviderFilterExpressionBuilderFunc(typeof(TEntity), Parameter, criterion);
                    if (num2 == 0)
                    {
                        expression2 = expression3;
                    }
                    else
                    {
                        if (expression2 != null) expression2 = Expression.AndAlso(expression2, expression3);
                    }

                    num2++;
                }

                if (num == 0)
                {
                    expression = expression2;
                }
                else if (expression2 != null)
                {
                    expression = Expression.OrElse(expression, expression2);
                }

                num++;
            }

            return expression;
        }

        //
        // Summary:
        //     Build order expression from order by options, to convert strings to
        //     lamda expression
        //
        // Parameters:
        //   OO:
        //     order by options
        //
        //   Query:
        //     Results after adding the order query part
        //
        // Type parameters:
        //   U:
        //     Order by field type
        //
        //   TEntity:
        //     Data set type
        public static void BuildOrderBy<U, TEntity>(Expression property, OrderByOptions OO, ref IQueryable<TEntity> Query)
        {
            Expression expression;
            var body = property;
            if (property == null)
            {
                expression = Expression.Parameter(typeof(TEntity), "");
                body = Expression.Property(expression, OO.MemberName);
            }
            else
            {
                expression = property;
                while (expression.NodeType != ExpressionType.Parameter)
                {
                    expression = ((MemberExpression)expression).Expression ?? throw new InvalidOperationException();
                }
            }

            Query = OO.SortOrder == OrderByOptions.Order.ASC
                ? Query.OrderBy(Expression.Lambda<Func<TEntity, U>>(body, (ParameterExpression)expression))
                : Query.OrderByDescending(Expression.Lambda<Func<TEntity, U>>(body, (ParameterExpression)expression));
        }

        //
        // Summary:
        //     Build then by order expression from order by options, to convert strings
        //     to lamda expression
        //
        // Parameters:
        //   OO:
        //     order by options
        //
        //   Query:
        //     Results after adding the order then by query part
        //
        // Type parameters:
        //   U:
        //     Order by field type
        //
        //   TEntity:
        //     Data set type
        public static void BuildThenBy<U, TEntity>(Expression property, OrderByOptions OO, ref IOrderedQueryable<TEntity> Query)
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity), "TEntity");
            var body = property;
            if (property == null)
            {
                parameterExpression = Expression.Parameter(typeof(TEntity), "");
                body = Expression.Property(parameterExpression, OO.MemberName);
            }

            if (OO.SortOrder == OrderByOptions.Order.ASC)
            {
                Query = Query.ThenBy(Expression.Lambda<Func<TEntity, U>>(body, parameterExpression));
            }
            else
            {
                Query = Query.ThenByDescending(Expression.Lambda<Func<TEntity, U>>(body, parameterExpression));
            }
        }

        //
        // Summary:
        //     Build order by then by expression from order by options, to convert
        //     strings to lamda expression
        //
        // Parameters:
        //   OO:
        //     order by options
        //
        //   Type:
        //     Sort member type
        //
        //   Query:
        //     Results after adding the order by query part
        //
        //   IsMainOrderBy:
        //     Is main order by
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        //
        // Exceptions:
        //   T:System.Exception:
        //     Unsupported sort Type parameter
        public static void BuildOrderByThenBy<TEntity>(Expression property, OrderByOptions OO, Type Type, ref IQueryable<TEntity> Query, bool IsMainOrderBy)
        {
            IOrderedQueryable<TEntity> Query2 = Query.OrderBy((p) => 0);
            if (Type == typeof(int))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<int, TEntity>(property, OO, ref Query);
                    return;
                }

                BuildThenBy<int, TEntity>(property, OO, ref Query2);
                Query = Query2;
            }

            if (Type == typeof(bool))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<bool, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query3 = Query.OrderBy(p => 0);
                BuildThenBy<bool, TEntity>(property, OO, ref Query3);
                Query = Query3;
                return;
            }

            if (Type == typeof(string))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<string, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query4 = Query.OrderBy(p => 0);
                BuildThenBy<string, TEntity>(property, OO, ref Query4);
                Query = Query4;
                return;
            }

            var Query5 = Query.OrderBy(p => 0);
            if (Type == typeof(DateTime?))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<DateTime?, TEntity>(property, OO, ref Query);
                    return;
                }

                BuildThenBy<DateTime?, TEntity>(property, OO, ref Query5);
                Query = Query5;
                return;
            }

            if (Type == typeof(DateTime))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<DateTime, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query6 = Query.OrderBy(p => 0);
                BuildThenBy<DateTime, TEntity>(property, OO, ref Query6);
                Query = Query6;
                return;
            }

            if (Type == typeof(byte))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<byte, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query7 = Query.OrderBy(p => 0);
                BuildThenBy<byte, TEntity>(property, OO, ref Query7);
                Query = Query7;
                return;
            }

            if (Type == typeof(short))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<short, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query8 = Query.OrderBy(p => 0);
                BuildThenBy<short, TEntity>(property, OO, ref Query8);
                Query = Query8;
                return;
            }

            if (Type == typeof(decimal))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<decimal, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query9 = Query.OrderBy(p => 0);
                BuildThenBy<decimal, TEntity>(property, OO, ref Query9);
                Query = Query9;
                return;
            }

            if (Type == typeof(Guid?))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<Guid?, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query10 = Query.OrderBy(p => 0);
                BuildThenBy<Guid?, TEntity>(property, OO, ref Query10);
                Query = Query10;
                return;
            }

            if (Type == typeof(Guid))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<Guid, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query11 = Query.OrderBy(p => 0);
                BuildThenBy<Guid, TEntity>(property, OO, ref Query11);
                Query = Query11;
                return;
            }

            if (Type == typeof(long))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<long, TEntity>(property, OO, ref Query);
                    return;
                }

                var Query10 = Query.OrderBy(p => 0);
                BuildThenBy<long, TEntity>(property, OO, ref Query10);
                Query = Query10;
                return;
            }

            var Query12 = Query.OrderBy(p => 0);
            if (Type == typeof(DateTimeOffset))
            {
                if (IsMainOrderBy)
                {
                    BuildOrderBy<DateTimeOffset, TEntity>(property, OO, ref Query);
                    return;
                }

                BuildThenBy<DateTimeOffset, TEntity>(property, OO, ref Query12);
            }

            throw new Exception($"Unsupported sort data-type: {Type}")
            {
                HelpLink = null,
                HResult = 0,
                Source = "ExtensionMethods.IQueryableExtensions"
            };
        }

        //
        // Summary:
        //     Build expression
        //
        // Parameters:
        //   feild:
        //     Data type feild name
        //
        //   value:
        //     Data type feild value
        //
        //   operator:
        //     Filter operator
        //
        // Type parameters:
        //   TEntity:
        //     Data set type
        //
        // Returns:
        //     Linq expression
        public static Expression<Func<TEntity, bool>> BuildExpression<TEntity>(string feild, object? value, eFilterOperator @operator = eFilterOperator.EqualsTo)
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity), "TEntity");
            var searchQuery = new SearchQuery(new FilterByOptions(feild, @operator, value))
            {
                Conditions = null
            };
            Expression expression = null;
            var num = 0;
            foreach (var condition in searchQuery.Conditions)
            {
                Expression expression2 = null!;
                var num2 = 0;
                foreach (var criterion in condition.Criteria)
                {
                    var expression3 = DatabaseProviderConfiguration.SearchProviderFilterExpressionBuilderFunc(typeof(TEntity), parameterExpression, criterion);
                    if (num2 == 0)
                    {
                        expression2 = expression3;
                    }
                    else
                    {
                        expression2 = Expression.AndAlso(expression2, expression3);
                    }

                    num2++;
                }

                if (num == 0)
                {
                    expression = expression2;
                }
                else
                {
                    expression = Expression.OrElse(expression, expression2);
                }

                num++;
            }

            return Expression.Lambda<Func<TEntity, bool>>(expression, parameterExpression);
        }

        //
        // Summary:
        //     Order By Property Name
        //
        // Parameters:
        //   query:
        //     Data set
        //
        //   propertyName:
        //     Order by property name
        //
        //   orderBy:
        //     Order by operator
        //
        // Type parameters:
        //   TSource:
        //     Data set type
        //
        // Returns:
        //     Ordered queryable
        public static IOrderedQueryable<TSource> OrderByPropertyName<TSource>(this IQueryable<TSource> query, string propertyName, OrderByOptions.Order orderBy)
        {
            var typeFromHandle = typeof(TSource);
            var propertyInfo = (from p in typeFromHandle.GetProperties()
                                where string.Equals(propertyName, p.Name, StringComparison.CurrentCultureIgnoreCase)
                                select p).FirstOrDefault();
            if (propertyInfo == null)
            {
                return (IOrderedQueryable<TSource>)query;
            }

            if (propertyInfo.DeclaringType != typeFromHandle)
            {
                propertyInfo = propertyInfo.DeclaringType!.GetProperty(propertyName);
            }

            var parameterExpression = Expression.Parameter(typeFromHandle, "x");
            var body = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
            var lambdaExpression = Expression.Lambda(body, parameterExpression);
            var methodName = orderBy == OrderByOptions.Order.ASC ? "OrderBy" : "OrderByDescending";
            var methodInfo = (from m in typeof(IQueryable).GetMethods()
                              where m.Name == methodName && m.IsGenericMethodDefinition
                              where m.GetParameters().ToList().Count == 2
                              select m).Single();
            var methodInfo2 = methodInfo.MakeGenericMethod(typeFromHandle, propertyInfo.PropertyType);
            return (IOrderedQueryable<TSource>)methodInfo2.Invoke(methodInfo2, new object[2] { query, lambdaExpression })! ?? throw new InvalidOperationException();
        }

        public static FilterByOptions CreateFilterByOptions(string @memberName, object? @filter, eFilterOperator @operator, List<Enum> @properties = null)
         => new()
         {
             FilterFor = @filter,
             FilterOperator = @operator,
             MemberName = @memberName,
             Properties = @properties
         };
    }
}
