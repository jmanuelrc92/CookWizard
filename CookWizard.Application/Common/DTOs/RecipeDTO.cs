using CookWizard.Application.Features.Recipes.Commands;

namespace CookWizard.Application.Common.DTOs;

public class RecipeDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Portions { get; set; }
    public string? Difficulty { get; set; }
    public int TotalTimeInSeconds { get; set; }
    public List<SectionDTO> Sections { get; set; }
}
