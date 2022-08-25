using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CCAS.Application.Common.Interfaces;
using CCAS.Infrastructure.Services;

namespace CCAS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddTransient<IDateTime, DateTimeService>();        

        return services;
    }
}
