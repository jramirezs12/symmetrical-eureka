using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace RulesEngine.Application.Abstractions.Behaviors
{
    internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
:       IPipelineBehavior<TRequest, TResponse>
            where TRequest : class
            where TResponse : IErrorOr
    {

        private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public RequestLoggingPipelineBehavior(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request,
                                      RequestHandlerDelegate<TResponse> next,
                                      CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            _logger.LogInformation("Procesing request {RequestName}", requestName);

            try
            {
                dynamic inputData = request;
                _logger.LogInformation("{RequestName} - Entry model  {Request}", requestName, (object)inputData.Value);
            }
            catch (Exception)
            {
                _logger.LogInformation("{RequestName} - Entry model  {Request}", requestName, request);
            }

            TResponse result = await next();

            try
            {
                dynamic response = result;
                _logger.LogInformation("{RequestName} - Model response  {Result}", requestName, (object)response.Value);
            }
            catch (Exception)
            {
                _logger.LogInformation("{RequestName} - Model response  {Result}", requestName, result);
            }

            if (result.IsError)
            {
                using (LogContext.PushProperty("Error", result.Errors, true))
                {
                    _logger.LogError("Completed Request {RequestName} with Error", requestName);
                }
            }
            _logger.LogInformation("Completed Request {RequestName}", requestName);
            return result;
        }
    }
}