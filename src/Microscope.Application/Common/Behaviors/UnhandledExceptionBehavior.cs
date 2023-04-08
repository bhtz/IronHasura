using FluentValidation;
using MediatR;
using Microscope.Application.Common.Exceptions;
using Microscope.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Microscope.Application.Common.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            return await next();
        }
        catch (DomainException ex)
        {
            _logger.LogError(ex, "DOMAIN EXCEPTION for Request {Name} {@Request}", requestName, request);
            _logger.LogError($"{ex.Message}");
            
            throw new ConflictException(ex.Message);
        }
        catch (PoliciesException ex)
        {
            _logger.LogError(ex, "POLICY EXCEPTION for Request {Name} {@Request}", requestName, request);
            _logger.LogError($"{ex.Message}");
            
            throw;
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "VALIDATION EXCEPTION for Request {Name} {@Request}", requestName, request);
            _logger.LogError($"{ex.Message}");
            
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UNHANDLED EXCEPTION for Request {Name} {@Request}", requestName, request);
            _logger.LogError($"{ex.Message}");

            throw;
        }
    }
}
