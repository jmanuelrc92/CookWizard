namespace CookWizard.Domain.Recipes.Models;

public class PreparationTime
{
    public int Value { get; set; }
    public string Unit { get; set; } = "minutes";
    public int Seconds { get; set; }
}
