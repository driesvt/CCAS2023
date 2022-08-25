using CCAS.Application.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CCAS.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (ValidationException ex)
        {
            _logger.LogError("CCAS Request: Validation Exception for Request {Name} {@Request} {@ValidationErrors}", typeof(TRequest).Name, request, ex.Errors);

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CCAS Request: Unhandled Exception for Request {Name} {@Request}", typeof(TRequest).Name, request);

            throw;
        }
    }
}
