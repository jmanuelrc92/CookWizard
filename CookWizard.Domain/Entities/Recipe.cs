namespace CookWizard.Domain.Entities;

public class Recipe
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<PreparationStep> PreparationSteps { get; set; } = new();
    public int Portions { get; set; }
    public PreparationTime PreparationTime { get; set; } = new();
}
