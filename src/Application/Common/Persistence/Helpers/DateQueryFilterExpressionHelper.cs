using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;


namespace CCAS.Application.Common.Persistence.Helpers;

public class DateQueryFilterExpressionHelper<T>
{
    const string dateWithHourMinuteSecondFilter = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
    const string dateWithHourMinuteFilter = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}";
    const string dateWithHourFilter = @"\d{4}-\d{2}-\d{2}\s\d{2}";
    const string dateFilter = @"\d{4}-\d{2}-\d{2}";
    const string monthFilter = @"\d{4}-\d{2}";
    const string yearFilter = @"\d{4}";

    private bool isGreaterThanExpression(string filter, out DateTime startDateRangeResult)
    {
        var regex = new Regex($@"^>\s*(?<datestart>{dateFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filterdate = match.Groups["datestart"].Value;
            DateTime parseResult;
            if (DateTime.TryParseExact(filterdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                startDateRangeResult = parseResult.Date.AddDays(1);
                return true;
            }
        }
        startDateRangeResult = default;
        return false;
    }

    private bool isGreaterThanOrEqualsExpression(string filter, out DateTime startDateRangeResult)
    {
        var regex = new Regex($@"^>=\s*(?<datestart>{dateFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filterdate = match.Groups["datestart"].Value;
            DateTime parseResult;
            if (DateTime.TryParseExact(filterdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                startDateRangeResult = parseResult.Date;
                return true;
            }
        }
        startDateRangeResult = default;
        return false;
    }

    private bool isLessThanExpression(string filter, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^<\s*(?<datestart>{dateFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filterdate = match.Groups["datestart"].Value;
            DateTime parseResult;
            if (DateTime.TryParseExact(filterdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                endDateRangeResultNextDay = parseResult.Date;
                return true;
            }
        }
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isLessThanOrEqualsExpression(string filter, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^<=\s*(?<datestart>{dateFilter})$");
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filterdate = match.Groups["datestart"].Value;
            DateTime parseResult;
            if (DateTime.TryParseExact(filterdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                endDateRangeResultNextDay = parseResult.Date.AddDays(1);
                return true;
            }
        }
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isRangeExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<datestart>{dateFilter})\s*\.\.\s*(?<dateend>{dateFilter})$");  //2015-02-10..2015-02-20
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filterdatefrom = match.Groups["datestart"].Value;
            var filterdateto = match.Groups["dateend"].Value;
            DateTime parseResultFrom;
            DateTime parseResultTo;
            if (DateTime.TryParseExact(filterdatefrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultFrom) &&
                DateTime.TryParseExact(filterdateto, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultTo))
            {
                startDateRangeResult = parseResultFrom.Date;
                endDateRangeResultNextDay = parseResultTo.Date.AddDays(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isMonthExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<month>{monthFilter})$");  //2015-02
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filtermonth = match.Groups["month"].Value;
            var filterdatefrom = $"{filtermonth}-01";
            DateTime parseResultFrom;
            if (DateTime.TryParseExact(filterdatefrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultFrom))
            {
                startDateRangeResult = parseResultFrom.Date;
                endDateRangeResultNextDay = startDateRangeResult.Date.AddMonths(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isMonthRangeExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<monthstart>{monthFilter})\s*\.\.\s*(?<monthend>{monthFilter})$");  //2015-02..2015-02
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filtermonthfrom = $"{match.Groups["monthstart"].Value}-01";
            var filtermonthto = $"{match.Groups["monthend"].Value}-01";
            DateTime parseResultFrom;
            DateTime parseResultTo;
            if (DateTime.TryParseExact(filtermonthfrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultFrom) &&
                DateTime.TryParseExact(filtermonthto, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultTo))
            {
                startDateRangeResult = parseResultFrom.Date;
                endDateRangeResultNextDay = parseResultTo.Date.AddMonths(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isYearExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<year>{yearFilter})$");  //2015
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filterdatefrom = $"{match.Groups["year"].Value}-01-01";
            DateTime parseResultFrom;
            if (DateTime.TryParseExact(filterdatefrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultFrom))
            {
                startDateRangeResult = parseResultFrom.Date;
                endDateRangeResultNextDay = startDateRangeResult.Date.AddYears(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isYearRangeExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<yearstart>{yearFilter})\s*\.\.\s*(?<yearend>{yearFilter})$");  //2015..2016
        var match = regex.Match(filter);
        if (match.Success)
        {
            var filteryearhfrom = $"{match.Groups["yearstart"].Value}-01-01";
            var filteryearto = $"{match.Groups["yearend"].Value}-01-01";
            DateTime parseResultFrom;
            DateTime parseResultTo;
            if (DateTime.TryParseExact(filteryearhfrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultFrom) &&
                DateTime.TryParseExact(filteryearto, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResultTo))
            {
                startDateRangeResult = parseResultFrom.Date;
                endDateRangeResultNextDay = parseResultTo.Date.AddYears(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isEqualToExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<date>{dateFilter})$");  //2015-01-02
        var match = regex.Match(filter);
        if (match.Success)
        {
            DateTime parseResult;
            if (DateTime.TryParseExact(match.Groups["date"].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                startDateRangeResult = parseResult.Date;
                endDateRangeResultNextDay = startDateRangeResult.Date.AddDays(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isHourExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<date>{dateWithHourFilter})$");  //2015-01-02 10
        var match = regex.Match(filter);
        if (match.Success)
        {
            DateTime parseResult;
            var valueToParse = $"{match.Groups["date"].Value}:00:00";
            if (DateTime.TryParseExact(valueToParse, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                startDateRangeResult = parseResult;
                endDateRangeResultNextDay = startDateRangeResult.AddHours(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isMinuteExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<date>{dateWithHourMinuteFilter})$");  //2015-01-02 10:01
        var match = regex.Match(filter);
        if (match.Success)
        {
            DateTime parseResult;
            var valueToParse = $"{match.Groups["date"].Value}:00";
            if (DateTime.TryParseExact(valueToParse, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                startDateRangeResult = parseResult;
                endDateRangeResultNextDay = startDateRangeResult.AddMinutes(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private bool isSecondExpression(string filter, out DateTime startDateRangeResult, out DateTime endDateRangeResultNextDay)
    {
        var regex = new Regex($@"^(?<date>{dateWithHourMinuteSecondFilter})$");  //2015-01-02 10:01:01
        var match = regex.Match(filter);
        if (match.Success)
        {
            DateTime parseResult;
            var valueToParse = $"{match.Groups["date"].Value}";
            if (DateTime.TryParseExact(valueToParse, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parseResult))
            {
                startDateRangeResult = parseResult;
                endDateRangeResultNextDay = startDateRangeResult.AddSeconds(1);
                return true;
            }
        }
        startDateRangeResult = default;
        endDateRangeResultNextDay = default;
        return false;
    }

    private IQueryable<T> expressionFilter(IQueryable<T> query, string propertyMemberName, string filter)
    {
        //Build paramater expression of new lambda being contructed
        var parameterExpression = Expression.Parameter(typeof(T));
        //Build the property being executed on 
        var propertyOrField = Expression.PropertyOrField(parameterExpression, propertyMemberName);

        //Using Expression Trees
        Expression? binaryExpression = null;
        Expression? binaryExpressionStartDate = null;
        Expression? binaryExpressionEndDateNextDay = null;

        DateTime searchStartDate;
        DateTime searchEndDateNextDay;

        var filterExpression = filter.Trim();

        //2015-01-01
        if (binaryExpression == null && isEqualToExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-01-02  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015-01-01..2015-01-02
        if (binaryExpression == null && isRangeExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-01-02  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015-01
        if (binaryExpression == null && isMonthExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-02-01  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015-01..2015-02
        if (binaryExpression == null && isMonthRangeExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-04-01  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015
        if (binaryExpression == null && isYearExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-04-01  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015..2016
        if (binaryExpression == null && isYearRangeExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-04-01  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015-01-01 10
        if (binaryExpression == null && isHourExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-01-02  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015-01-01 10:01
        if (binaryExpression == null && isMinuteExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-01-02  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //2015-01-01 10:01:01
        if (binaryExpression == null && isSecondExpression(filterExpression, out searchStartDate, out searchEndDateNextDay))
        {
            binaryExpressionStartDate = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
            binaryExpressionEndDateNextDay = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-01-02  (to include all time in the day)
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }
        //>2015-01-01
        if (binaryExpression == null && isGreaterThanExpression(filterExpression, out searchStartDate))
        {
            binaryExpression = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty > 2015-01-01
        }
        //>=2015-01-01
        if (binaryExpression == null && isGreaterThanOrEqualsExpression(filterExpression, out searchStartDate))
        {
            binaryExpression = myGreaterThanOrEqual(propertyOrField, Expression.Constant(searchStartDate));  // p => p.dateProperty >= 2015-01-01
        }
        //<2015-01-01
        if (binaryExpression == null && isLessThanExpression(filterExpression, out searchEndDateNextDay))
        {
            binaryExpression = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-01-01
        }
        //<=2015-01-01
        if (binaryExpression == null && isLessThanOrEqualsExpression(filterExpression, out searchEndDateNextDay))
        {
            binaryExpression = myLessThan(propertyOrField, Expression.Constant(searchEndDateNextDay));  // p => p.dateProperty < 2015-01-01
        }
        //No matched expression. Create filter that will not match anything.
        if (binaryExpression == null)
        {
            binaryExpressionStartDate = Expression.Equal(propertyOrField, Expression.Constant(DateTime.Now));
            binaryExpressionEndDateNextDay = Expression.Equal(propertyOrField, Expression.Constant(default(DateTime)));
            binaryExpression = Expression.AndAlso(binaryExpressionStartDate, binaryExpressionEndDateNextDay);
        }

        if (binaryExpression != null)
        {
            var myExpr = Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
            query = query.Where(myExpr);
        }

        return query;
    }

    public IQueryable<T> ExpressionFilter(IQueryable<T> query, Expression<Func<T, DateTime>> property, string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return query;


        //Get the body of the predicate
        var memberExpression = (MemberExpression)property.Body;
        //Get the predicate's property member name
        var propertyMemberName = memberExpression.Member.Name;

        return expressionFilter(query, propertyMemberName, filter);
    }

    public IQueryable<T> ExpressionFilter(IQueryable<T> query, Expression<Func<T, DateTime?>> property, string filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return query;


        //Get the body of the predicate
        var memberExpression = (MemberExpression)property.Body;
        //Get the predicate's property member name
        var propertyMemberName = memberExpression.Member.Name;

        return expressionFilter(query, propertyMemberName, filter);
    }

    private Expression myGreaterThanOrEqual(Expression e1, Expression e2)
    {
        if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
            e2 = Expression.Convert(e2, e1.Type);
        else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
            e1 = Expression.Convert(e1, e2.Type);
        return Expression.GreaterThanOrEqual(e1, e2);
    }
    private Expression myLessThan(Expression e1, Expression e2)
    {
        if (IsNullableType(e1.Type) && !IsNullableType(e2.Type))
            e2 = Expression.Convert(e2, e1.Type);
        else if (!IsNullableType(e1.Type) && IsNullableType(e2.Type))
            e1 = Expression.Convert(e1, e2.Type);
        return Expression.LessThan(e1, e2);
    }
    private bool IsNullableType(Type t)
    {
        return t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
