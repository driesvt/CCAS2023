using CCAS.Application.Common.Models;
using System.Linq.Expressions;

namespace CCAS.Application.Common.Persistence.Helpers;

public static class QueryableExtensionMethods
{
    public static IQueryable<TSource> NumericExpressionFilter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> predicate, string filter)
    {
        var queryHelper = new NumericQueryFilterExpressionHelper<TSource, decimal>();
        return queryHelper.ExpressionFilter(source, predicate, filter);
    }

    public static IQueryable<TSource> NumericExpressionFilter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> predicate, string filter)
    {
        var queryHelper = new NumericQueryFilterExpressionHelper<TSource, int>();
        return queryHelper.ExpressionFilter(source, predicate, filter);
    }

    public static IQueryable<TSource> NumericExpressionFilter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> predicate, string filter)
    {
        var queryHelper = new NumericQueryFilterExpressionHelper<TSource, double>();
        return queryHelper.ExpressionFilter(source, predicate, filter);
    }

    public static IQueryable<TSource> StringExpressionFilter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, string?>> predicate, string filter)
    {
        var stringQueryHelper = new StringQueryFilterExpressionHelper<TSource>();
        return stringQueryHelper.ExpressionFilter(source, predicate, filter);
    }

    public static IQueryable<TSource> DateExpressionFilter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, DateTime>> predicate, string filter)
    {
        var dateQueryHelper = new DateQueryFilterExpressionHelper<TSource>();
        return dateQueryHelper.ExpressionFilter(source, predicate, filter);
    }

    public static IQueryable<TSource> DateExpressionFilter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, DateTime?>> predicate, string filter)
    {
        var dateQueryHelper = new DateQueryFilterExpressionHelper<TSource>();
        return dateQueryHelper.ExpressionFilter(source, predicate, filter);
    }


    public static IQueryable<TSource> WhereInListFilter<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> predicate, IEnumerable<int> values)
    {
        var queryHelper = new WhereInListFilterExpressionHelper<TSource, int>();
        return queryHelper.ExpressionFilter(source, predicate, values);
    }

    public static IQueryable<TModel> ApplySortOptions<TModel>(this IQueryable<TModel> query, IEnumerable<SortOption<TModel>> sortList)
    {
        var i = 0;
        IOrderedQueryable<TModel>? orderedQuery = null;
        foreach (var sort in sortList)
        {
            i++;
            if (i == 1)
                orderedQuery = sort.SortOrder == Enums.SortOrder.Ascending ? query.OrderBy(sort.KeySelector) : query.OrderByDescending(sort.KeySelector);
            else
                orderedQuery = sort.SortOrder == Enums.SortOrder.Ascending ? orderedQuery!.ThenBy(sort.KeySelector) : orderedQuery!.ThenByDescending(sort.KeySelector);
        }
        return orderedQuery!;
    }
}
