using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CCAS.Application.Common.Persistence.Helpers;

public class WhereInListFilterExpressionHelper<T, Y> where Y : struct,
                                                        IComparable,
                                                        IComparable<Y>,
                                                        IConvertible,
                                                        IEquatable<Y>,
                                                        IFormattable
{
    public IQueryable<T> ExpressionFilter(IQueryable<T> query, Expression<Func<T, Y>> property, IEnumerable<Y> values)
    {
        if (property == null)
            return query;
        if (values == null)
            return query;

        //Get the body of the predicate
        var memberExpression = (MemberExpression)property.Body;
        //Get the predicate's property member name
        var propertyMemberName = memberExpression.Member.Name;
        //Build paramater expression of new lambda being contructed
        var parameterExpression = Expression.Parameter(typeof(T));  // p =>
        //Build the property being executed on 
        var propertyOrField = Expression.PropertyOrField(parameterExpression, propertyMemberName); // p.property

        if (!values.Any())
            return query;

        /*
        foreach (var item in values)
        {
            if (binaryExpression == null)
                binaryExpression = Expression.Equal(propertyOrField, Expression.Constant(item));
            else
            {
                binaryExpressionSecond = Expression.Equal(propertyOrField, Expression.Constant(item));
                binaryExpression = Expression.Or(binaryExpression, binaryExpressionSecond);
            }
        }
        var myExpr = Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
        */

        var equals = values.Select(value => (Expression)Expression.Equal(propertyOrField, Expression.Constant(value, typeof(Y))));
        var binaryExpression = equals.Aggregate((accumulate, equal) => Expression.Or(accumulate, equal));

        var myExpr = Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
        query = query.Where(myExpr);

        return query;
    }

    public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(
        Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
    {
        if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
        if (null == values) { throw new ArgumentNullException("values"); }
        var p = valueSelector.Parameters.Single();
        // p => valueSelector(p) == values[0] || valueSelector(p) == ...
        if (!values.Any())
        {
            return e => false;
        }
        var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
        var body = equals.Aggregate((accumulate, equal) => Expression.Or(accumulate, equal));
        return Expression.Lambda<Func<TElement, bool>>(body, p);
    }
}
