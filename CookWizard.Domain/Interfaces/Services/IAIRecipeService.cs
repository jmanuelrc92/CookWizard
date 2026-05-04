using CookWizard.Domain.Entities;

namespace CookWizard.Domain.Interfaces.Services;

public interface IAIRecipeService
{
    Task<RecipeSearchCriteria> ParsePrompt(string prompt);
}
