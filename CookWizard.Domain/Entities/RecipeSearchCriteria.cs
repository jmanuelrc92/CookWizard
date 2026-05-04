namespace CookWizard.Domain.Entities;

public class RecipeSearchCriteria
{
    public List<string>? Ingredients { get; set; }
    public string? Difficulty { get; set; }
    public int? MaxTime { get; set; }
}
