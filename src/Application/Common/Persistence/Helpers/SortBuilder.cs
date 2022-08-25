using CCAS.Application.Common.Enums;
using CCAS.Application.Common.Models;
using System.Linq.Expressions;

namespace CCAS.Application.Common.Persistence.Helpers;

public class SortBuilder<T>
{
    public IEnumerable<SortOption<T>> CreateSortList(IEnumerable<string>? sortOptions = null, SortOption<T>? defaultSortOption = null)
    {
        var parameterExpression = Expression.Parameter(typeof(T));
        var finalSortOrder = new List<SortOption<T>>();

        // Fallback to default, if possible, if no sorts have been supplied
        if (sortOptions == null || sortOptions.Count() == 0)
        {
            if (defaultSortOption == null)
                return null!;
            else
            {
                finalSortOrder.Add(defaultSortOption);
                return finalSortOrder;
            }
        }


        foreach (var currentSortField in sortOptions)
        {
            if (!string.IsNullOrWhiteSpace(currentSortField))
            {
                var field = getFieldAndOrderFromString(currentSortField);
                try
                {
                    var result = getSortOption(parameterExpression, field.field, field.order);
                    finalSortOrder.Add(result);
                }
                catch (Exception)
                {
                    // Ignore invalid sort options
                }
            }
        }

        // Fallback to default if no sort has been appied
        if (finalSortOrder.Count() == 0 && defaultSortOption != null)
            finalSortOrder.Add(defaultSortOption);

        if (finalSortOrder.Count() == 0)
            finalSortOrder.Add(defaultSortOption!);

        return finalSortOrder;
    }

    private (string field, SortOrder order) getFieldAndOrderFromString(string sortField)
    {
        var sortOrder = SortOrder.Ascending;
        var field = "";

        switch (sortField.Trim())
        {
            case string c when c.StartsWith("+"):
                sortOrder = SortOrder.Ascending;
                field = sortField.Trim().TrimStart('+');
                break;
            case string c when c.StartsWith("-"):
                sortOrder = SortOrder.Descending;
                field = sortField.Trim().TrimStart('-');
                break;
            default:
                sortOrder = SortOrder.Ascending;
                field = sortField.Trim();
                break;
        }
        return (field, sortOrder);
    }

    private SortOption<T> getSortOption(ParameterExpression parameterExpression, string field, SortOrder order)
    {
        var result = getPropertyFromString(parameterExpression, field);
        try
        {
            Expression conversion = Expression.Convert(result, typeof(object));
            return new SortOption<T>(Expression.Lambda<Func<T, object>>(conversion, parameterExpression), order);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private MemberExpression getPropertyFromString(ParameterExpression parameterExpression, string propertyName)
    {
        MemberExpression propertyOrField;
        try
        {
            propertyOrField = Expression.PropertyOrField(parameterExpression, propertyName);
            return propertyOrField;
        }
        catch (Exception)
        {
            throw;
        }
    }

}
