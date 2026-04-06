using CookWizard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected ActionResult HandleResult<T>(CookWizardApiResult<T> result)
    {
        if (result.IsFailure)
        {
            if (result.Error.Contains("no existe"))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
        return Ok(result);
    }
}
