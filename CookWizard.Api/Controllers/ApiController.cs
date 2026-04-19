using CookWizard.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected ActionResult HandleResult<T>(ResultObject<T> result)
    {
        if (!result.IsFailure)
            return Ok(result);
        if (result.Error.Contains("no existe"))
            return NotFound(result);
        return BadRequest(result);
    }
}
