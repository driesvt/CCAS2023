using CCAS.Application.Common.Interfaces;
using CCAS.Application.Common.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CCAS.Application.Common.Behaviours;

public class LogScopeBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public LogScopeBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        string userName = _currentUserService.UserName ?? string.Empty;
       
        TResponse response;

        using (_logger.Start()
            .Add("RequestName", requestName)
            .Add("RequestTraceId", Guid.NewGuid().ToString())
            .Add("UserName", userName)
            .BuildScope())
        {
            response = await next();
        }

        return response;

    }
}
