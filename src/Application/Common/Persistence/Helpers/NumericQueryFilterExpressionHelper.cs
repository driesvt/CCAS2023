using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CCAS.Application.Common.Persistence.Helpers;

public class NumericQueryFilterExpressionHelper<T, Y> where Y : struct,
                                                        IComparable,
                                                        IComparable<Y>,
                                                        IConvertible,
                                                        IEquatable<Y>,
                                                        IFormattable
{
    const string NumberFilter = @"-?\d+(\.\d+)?";

    private static bool tryParse(string stringToParse, out Y returnValue)
    {
        var converter = TypeDescriptor.GetConverter(typeof(Y));
        if (converter != null)
        {
            try
            {
                returnValue = (Y)converter.ConvertFromString(stringToParse)!;
                return true;
            }
            catch (Exception)
            {
                returnValue = default;
                return false;
            }
        }
        returnValue = default;
        return false;
    }

    private static bool isGreaterThanExpression(string filter, out Y number)
    {
        var regex = new Regex($@"^>\s*(?<filternumber>{NumberFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filternumber = match.Groups["filternumber"].Value;
            if (NumericQueryFilterExpressionHelper<T, Y>.tryParse(filternumber, out var parseResult))
            {
                number = parseResult;
                return true;
            }
        }
        number = default;
        return false;
    }

    private static bool isGreaterThanOrEqualsExpression(string filter, out Y number)
    {
        var regex = new Regex($@"^>=\s*(?<filternumber>{NumberFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filternumber = match.Groups["filternumber"].Value;
            if (NumericQueryFilterExpressionHelper<T, Y>.tryParse(filternumber, out var parseResult))
            {
                number = parseResult;
                return true;
            }
        }
        number = default;
        return false;
    }

    private static bool isLessThanExpression(string filter, out Y number)
    {
        var regex = new Regex($@"^<\s*(?<filternumber>{NumberFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filternumber = match.Groups["filternumber"].Value;
            if (NumericQueryFilterExpressionHelper<T, Y>.tryParse(filternumber, out var parseResult))
            {
                number = parseResult;
                return true;
            }
        }
        number = default;
        return false;
    }

    private static bool isLessThanOrEqualsExpression(string filter, out Y number)
    {
        var regex = new Regex($@"^<=\s*(?<filternumber>{NumberFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filternumber = match.Groups["filternumber"].Value;
            if (NumericQueryFilterExpressionHelper<T, Y>.tryParse(filternumber, out var parseResult))
            {
                number = parseResult;
                return true;
            }
        }
        number = default;
        return false;
    }

    private static bool isRangeExpression(string filter, out Y numberFrom, out Y numberTo)
    {
        var regex = new Regex($@"^(?<filternumberfrom>{NumberFilter})\s*\.\.\s*(?<filternumberto>{NumberFilter})$");  //10000..12000
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filternumberfrom = match.Groups["filternumberfrom"].Value;
            var filternumberto = match.Groups["filternumberto"].Value;
            if (NumericQueryFilterExpressionHelper<T, Y>.tryParse(filternumberfrom, out var parseResultFrom) && NumericQueryFilterExpressionHelper<T, Y>.tryParse(filternumberto, out var parseResultTo))
            {
                numberFrom = parseResultFrom;
                numberTo = parseResultTo;
                return true;
            }
        }
        numberFrom = default;
        numberTo = default;
        return false;
    }

    private static bool isEqualToExpression(string filter, out Y number)
    {
        if (NumericQueryFilterExpressionHelper<T, Y>.tryParse(filter, out var parseResult))
        {
            number = parseResult;
            return true;
        }
        number = default;
        return false;
    }

    public IQueryable<T> ExpressionFilter(IQueryable<T> query, Expression<Func<T, Y>> property, string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return query;


        //Get the body of the predicate
        var memberExpression = (MemberExpression)property.Body;
        //Get the predicate's property member name
        var propertyMemberName = memberExpression.Member.Name;
        //Build paramater expression of new lambda being contructed
        var parameterExpression = Expression.Parameter(typeof(T));  // p =>
        //Build the property being executed on 
        var propertyOrField = Expression.PropertyOrField(parameterExpression, propertyMemberName); // p.property

        //Using Expression Trees
        Expression? binaryExpression = null;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
        Expression? binaryExpressionSecond = null;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

        var filterExpression = filter.Trim();
        if (binaryExpression == null && NumericQueryFilterExpressionHelper<T, Y>.isEqualToExpression(filterExpression, out var parseResult))
        {
            binaryExpression = Expression.Equal(propertyOrField, Expression.Constant(parseResult));
        }
        if (binaryExpression == null && NumericQueryFilterExpressionHelper<T, Y>.isGreaterThanExpression(filterExpression, out parseResult))
        {
            binaryExpression = Expression.GreaterThan(propertyOrField, Expression.Constant(parseResult));  // p => p.property > 100
        }
        if (binaryExpression == null && NumericQueryFilterExpressionHelper<T, Y>.isGreaterThanOrEqualsExpression(filterExpression, out parseResult))
        {
            binaryExpression = Expression.GreaterThanOrEqual(propertyOrField, Expression.Constant(parseResult)); // p => p.property >= 100
        }
        if (binaryExpression == null && NumericQueryFilterExpressionHelper<T, Y>.isLessThanExpression(filterExpression, out parseResult))
        {
            binaryExpression = Expression.LessThan(propertyOrField, Expression.Constant(parseResult));  // p => p.property < 100
        }
        if (binaryExpression == null && NumericQueryFilterExpressionHelper<T, Y>.isLessThanOrEqualsExpression(filterExpression, out parseResult))
        {
            binaryExpression = Expression.LessThanOrEqual(propertyOrField, Expression.Constant(parseResult));  // p => p.property <= 100
        }
        if (binaryExpression == null && NumericQueryFilterExpressionHelper<T, Y>.isRangeExpression(filterExpression, out var parseResultRangeFrom, out var parseResultRangeTo))
        {
            binaryExpression = Expression.GreaterThanOrEqual(propertyOrField, Expression.Constant(parseResultRangeFrom));
            binaryExpressionSecond = Expression.LessThanOrEqual(propertyOrField, Expression.Constant(parseResultRangeTo));
            binaryExpression = Expression.AndAlso(binaryExpression, binaryExpressionSecond);  // p => p.property >= 100 AND p.property <= 500
        }

        if (binaryExpression == null)
        {
            //If it did not match anything, create an expression that WILL NOT MATCH ANYTHING
            _ = NumericQueryFilterExpressionHelper<T, Y>.tryParse("0", out var value1);
            _ = NumericQueryFilterExpressionHelper<T, Y>.tryParse("1", out var value2);

            binaryExpression = Expression.Equal(propertyOrField, Expression.Constant(value1));
            binaryExpressionSecond = Expression.Equal(propertyOrField, Expression.Constant(value2));
            binaryExpression = Expression.AndAlso(binaryExpression, binaryExpressionSecond);
        }

        var myExpr = Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
        query = query.Where(myExpr);

        return query;
    }


}
