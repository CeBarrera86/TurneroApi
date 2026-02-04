using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TurneroApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ErrorController : ControllerBase
  {
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(IWebHostEnvironment environment, ILogger<ErrorController> logger)
    {
      _environment = environment;
      _logger = logger;
    }

    [HttpGet]
    [Route("")]
    public IActionResult HandleError()
    {
      var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
      var exception = context?.Error;

      if (exception != null)
      {
        _logger.LogError(exception, "Error no controlado en {Path}", HttpContext.Request.Path);
      }

      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status500InternalServerError,
        Title = "Ha ocurrido un error inesperado.",
        Detail = _environment.IsDevelopment() ? exception?.Message : "Contacte al administrador del sistema.",
        Instance = HttpContext.Request.Path
      };

      problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;

      return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
    }
  }
}