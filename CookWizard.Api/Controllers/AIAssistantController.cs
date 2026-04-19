using CookWizard.Application.Features.Recipes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CookWizard.Api.Controllers;

[Route("api/[controller]")]
public class AIAssistantController : ApiController
{
    private readonly IMediator _mediator;

    public AIAssistantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("question")]
    public async Task<ActionResult> Question(string question)
    {
        var result = await _mediator.Send(new AIAssistantQuestionQuery(question));
        return HandleResult(result);
    }
}