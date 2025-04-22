using Battle_Spells.Api.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Battle_Spells.Api.Http.Handlers
{
    public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, $"Exception occurred: {exception.Message}");

            ProblemDetails problemDetails = null;

            if (exception is APIException apiException)
            {
                problemDetails = new ProblemDetails
                {
                    Status = (int)apiException.StatusCode,
                    Title = exception.Message,
                    Instance = httpContext.GetEndpoint()?.DisplayName,
                };
            }
            else
            {
                problemDetails = new ProblemDetails
                {
                    Status = null,
                    Title = exception.Message,
                    Instance = httpContext.GetEndpoint()?.DisplayName,
                };
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
