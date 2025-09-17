using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace SharedKernel
{
    public class SqlSearchProvider : IDisposable
    {
        public SqlSearchProvider()
        {
            DatabaseProviderConfiguration.SearchProviderFilterExpressionBuilderFunc = BuildFilterExpression;
        }

        /// <summary>
        /// Build search expression from  filter by options, to convert strings to lamda expression
        /// </summary>
        /// <typeparam name="TEntity">Data set type</typeparam>
        /// <param name="parameter">Root expression</param>
        /// <param name="filterByOptions">> filter by options</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">When the  filter operator not implemented</exception>
        public static Expression BuildFilterExpression(Type entity, Expression parameter, FilterByOptions filterByOptions)
        {
            Expression member;
            Type memberType;
            if (filterByOptions.MemberName.Contains("."))
            {
                var mems = filterByOptions.MemberName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                member = parameter;
                memberType = entity;
                foreach (var _s in mems)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(memberType) && memberType != typeof(string))
                    {
                        return EmbededListFilter(filterByOptions, member, memberType, _s);
                    }
                    else
                    {
                        member = Expression.Property(member, _s);
                        memberType = memberType.GetProperty(_s, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.PropertyType ?? throw new InvalidOperationException();
                    }
                }
            }
            else
            {
                member = Expression.Property(parameter, filterByOptions.MemberName);
                memberType = entity.GetProperty(filterByOptions.MemberName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)?.PropertyType ?? throw new InvalidOperationException();
            }
            Expression constant = null;

            var constcoll = filterByOptions.FilterFor as ICollection;
            if (constcoll == null)
            {
                constant = Expression.Constant(filterByOptions.FilterFor.ConvertObjectToType(memberType));
                constant = Expression.Convert(constant, memberType);
            }

            switch (filterByOptions.FilterOperator)
            {
                case eFilterOperator.BeginsWith:
                case eFilterOperator.Contains:
                case eFilterOperator.NotBeginsWith:
                case eFilterOperator.NotContains:
                    {
                        string _method = "";
                        switch (filterByOptions.FilterOperator)
                        {
                            case eFilterOperator.BeginsWith:
                            case eFilterOperator.NotBeginsWith:
                                {
                                    _method = "StartsWith";
                                    break;
                                }

                            case eFilterOperator.Contains:
                            case eFilterOperator.NotContains:
                                {
                                    _method = "Contains";
                                    break;
                                }
                        }
                        var method = typeof(string).GetMethod(_method, new[] { typeof(string) });

                        if (filterByOptions.FilterOperator is eFilterOperator.NotBeginsWith or eFilterOperator.NotContains)
                            return Expression.Not(Expression.Call(member, method, Expression.Constant(filterByOptions.FilterFor, filterByOptions.FilterFor?.GetType() ?? throw new InvalidOperationException())));
                        else
                            return Expression.Call(member, method, Expression.Constant(filterByOptions.FilterFor, filterByOptions.FilterFor?.GetType() ?? throw new InvalidOperationException()));
                    }

                case eFilterOperator.EqualsTo:
                    {
                        if (constant != null) return Expression.Equal(member, constant);
                        break;
                    }

                case eFilterOperator.NotEqualsTo:
                    {
                        if (constant != null) return Expression.NotEqual(member, constant);
                        break;
                    }

                case eFilterOperator.LessThan:
                    {
                        if (constant != null) return Expression.LessThan(member, constant);
                        break;
                    }

                case eFilterOperator.LessThanOrEquals:
                    {
                        if (constant != null) return Expression.LessThanOrEqual(member, constant);
                        break;
                    }

                case eFilterOperator.GreaterThan:
                    {
                        if (constant != null) return Expression.GreaterThan(member, constant);
                        break;
                    }

                case eFilterOperator.GreaterThanOrEquals:
                    {
                        if (constant != null) return Expression.GreaterThanOrEqual(member, constant);
                        break;
                    }

                case eFilterOperator.EqualsToList:
                case eFilterOperator.NotEqualsToList:
                    {
                        if (constcoll is { Count: > 0 })
                        {
                            var method = typeof(Enumerable).GetMethods().First(o => o.Name == "Contains" & o.GetParameters().Count() == 2);
                            method = method.MakeGenericMethod(memberType);
                            var t1 = typeof(List<>);
                            var iEnumerable = (IList)Activator.CreateInstance(t1.MakeGenericType(new Type[] { memberType }))!;
                            foreach (var _i in constcoll)
                                iEnumerable.Add(TypeDescriptor.GetConverter(memberType).ConvertFrom(_i));
                            filterByOptions.FilterFor = iEnumerable;

                            var expression = Expression.Call(method, new[] { Expression.Constant(filterByOptions.FilterFor), member });

                            if (filterByOptions.FilterOperator == eFilterOperator.NotEqualsToList)
                                return Expression.Not(expression);

                            return expression;
                        }

                        return null;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            return null;
        }

        protected static MethodCallExpression EmbededListFilter(FilterByOptions filterByOptions, Expression parameter, Type entity, string innerMemberName)
        {
            Expression member = parameter;
            Type memberType = entity;
            var elementType = memberType.GetGenericArguments().FirstOrDefault();
            if (elementType == null)
                throw new InvalidOperationException("Element type cannot be determined for the collection.");

            var innerParameter = Expression.Parameter(elementType, "x");
            var innerMember = Expression.Property(innerParameter, innerMemberName.ToString());

            Expression _constant = null;
            var _constcoll = filterByOptions.FilterFor as ICollection;
            if (_constcoll == null)
            {
                _constant = Expression.Constant(filterByOptions.FilterFor.ConvertObjectToType(innerMember.Type));
                _constant = Expression.Convert(_constant, innerMember.Type);
            }

            // Create expression x.Id == value
            Expression filterExpression = null;
            switch (filterByOptions.FilterOperator)
            {
                case eFilterOperator.EqualsTo:
                    filterExpression = Expression.Equal(innerMember, _constant);
                    break;
                case eFilterOperator.NotEqualsTo:
                    filterExpression = Expression.NotEqual(innerMember, _constant);
                    break;
                case eFilterOperator.BeginsWith:
                    filterExpression = Expression.Call(innerMember, "StartsWith", null, _constant);
                    break;
                case eFilterOperator.Contains:
                    filterExpression = Expression.Call(innerMember, "Contains", null, _constant);
                    break;
                case eFilterOperator.GreaterThan:
                    filterExpression = Expression.GreaterThan(innerMember, _constant);
                    break;
                case eFilterOperator.GreaterThanOrEquals:
                    filterExpression = Expression.GreaterThanOrEqual(innerMember, _constant);
                    break;
                case eFilterOperator.LessThan:
                    filterExpression = Expression.LessThan(innerMember, _constant);
                    break;
                case eFilterOperator.LessThanOrEquals:
                    filterExpression = Expression.LessThanOrEqual(innerMember, _constant);
                    break;
                case eFilterOperator.EqualsToList:
                    var filterForType = filterByOptions.FilterFor.GetType();
                    if (typeof(IEnumerable).IsAssignableFrom(filterForType) && filterForType != typeof(string))
                    {
                        var containsExpression = Expression.Call(
                            typeof(Enumerable),
                            "Contains",
                            new[] { innerMember.Type },
                            Expression.Constant(filterByOptions.FilterFor/*enumerableFilter*/),
                            innerMember);

                        filterExpression = containsExpression;
                    }
                    break;
                case eFilterOperator.NotEqualsToList:
                    var filterForTypeNot = filterByOptions.FilterFor.GetType();
                    if (typeof(IEnumerable).IsAssignableFrom(filterForTypeNot) && filterForTypeNot != typeof(string))
                    {
                        var containsExpression = Expression.Call(
                            typeof(Enumerable),
                            "Contains",
                            new[] { innerMember.Type },
                            Expression.Constant(filterByOptions.FilterFor),
                            innerMember);

                        filterExpression = Expression.Not(containsExpression);
                    }
                    break;
                case eFilterOperator.NotBeginsWith:
                    filterExpression = Expression.Not(Expression.Call(innerMember, "StartsWith", null, _constant));
                    break;
                case eFilterOperator.NotContains:
                    filterExpression = Expression.Not(Expression.Call(innerMember, "Contains", null, _constant));
                    break;
                default:
                    throw new NotSupportedException($"Filter operator {filterByOptions.FilterOperator} is not supported.");
            }
            // Build the .Where(x => x.Id == value) expression
            var whereCallExpression = Expression.Call(
                typeof(Enumerable),
                "Where",
                new Type[] { elementType },
                member,
                Expression.Lambda(filterExpression, innerParameter)
            );

            // Return the .Any() expression to check if any elements satisfy the condition
            var anyExpression = Expression.Call(
                typeof(Enumerable),
                "Any",
                new Type[] { elementType },
                whereCallExpression
            );
            return anyExpression;
        }

        public void Dispose()
        {
            DatabaseProviderConfiguration.SearchProviderFilterExpressionBuilderFunc = null;
        }
    }
}