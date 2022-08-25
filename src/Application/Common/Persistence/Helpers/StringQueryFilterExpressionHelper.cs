using Baseline.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCAS.Application.Common.Persistence.Helpers;

public class StringQueryFilterExpressionHelper<T>
{
    private bool isLiteralString(string filter, out string searchString)
    {
        var regex = new Regex($@"^""(?<searchstring>.*)""$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            searchString = match.Groups["searchstring"].Value;
            return true;
        }
        searchString = "";
        return false;
    }

    public IQueryable<T> ExpressionFilter(IQueryable<T> query, Expression<Func<T, string?>> property, string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return query;

        //Get the body of the predicate
        var memberExpression = (MemberExpression)property.Body;
        //Get the predicate's property member name
        var propertyMemberName = memberExpression.Member.Name;
        //Build paramater expression of new lambda being contructed
        var parameterExpression = Expression.Parameter(typeof(T));
        //Build the property being executed on 
        var propertyOrField = Expression.PropertyOrField(parameterExpression, propertyMemberName);

        //Using Expression Trees
        Expression? binaryExpression = null;

        string searchString;

        var filterExpression = filter.Trim();

        if (binaryExpression == null && isLiteralString(filterExpression, out searchString))
        {
            binaryExpression = Expression.Equal(propertyOrField, Expression.Constant(searchString));  // p => p.property == "mystring"
        }

        if (binaryExpression == null)
        {
            var method = ReflectionHelper.GetMethod<string>(s => s.Contains("a"));
            binaryExpression = Expression.Call(propertyOrField, method, Expression.Constant(filterExpression));  // p => p.property.Contains("mystring")
        }

        if (binaryExpression != null)
        {
            var myExpr = Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
            query = query.Where(myExpr);
        }

        return query;
    }

}
