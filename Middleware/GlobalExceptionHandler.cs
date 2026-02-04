using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TurneroApi.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
  private readonly ILogger<GlobalExceptionHandler> _logger;
  private readonly IWebHostEnvironment _environment;

  public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment)
  {
    _logger = logger;
    _environment = environment;
  }

  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    _logger.LogError(exception, "Error no controlado en {Path}", httpContext.Request.Path);

    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status500InternalServerError,
      Title = "Ha ocurrido un error inesperado.",
      Detail = _environment.IsDevelopment() ? exception.Message : "Contacte al administrador del sistema.",
      Instance = httpContext.Request.Path
    };

    problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    return true;
  }
}
