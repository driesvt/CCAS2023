using CCAS.Application.Common.Interfaces;

namespace CCAS.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
