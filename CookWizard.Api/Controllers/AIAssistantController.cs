using CookWizard.Application.Common;
using CookWizard.Application.Features.Recipes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Controllers;

[Route("api/[controller]")]
public class AIAssistantController : ApiController
{
    private readonly IMediatorCustom _mediator;

    public AIAssistantController(IMediatorCustom mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("question")]
    public async Task<ActionResult> Question(string question)
    {
        var result = await _mediator.SendAsync(new AIAssistantQuestionQuery(question));
        return HandleResult(result);
    }
}