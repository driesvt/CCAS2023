using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CCAS.Application.Common.Logging;
public static class LoggerExtensions
{
    public static IDisposable BeginScope(this ILogger logger, string key, object value)
    {
        return logger.BeginScope(new Dictionary<string, object> { { key, value } });
    }

    public static (Dictionary<string, object> dictionery, ILogger logger) Start(this ILogger logger)
    {
        return (new Dictionary<string, object>(), logger);
    }

    public static (Dictionary<string, object> dictionery, ILogger logger) Add(this (Dictionary<string, object> dictionery, ILogger logger) tuple, string key, object value)
    {
        tuple.dictionery.Add(key, value);
        return tuple;
    }

    public static IDisposable BuildScope(this (Dictionary<string, object> dictionery, ILogger logger) tuple)
    {
        return tuple.logger.BeginScope(tuple.dictionery);
    }

}
