namespace CookWizard.Domain.Entities;

public class Ingredient
{
    public double Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Product { get; set; } = string.Empty;
    public string PreparationNotes { get; set; } = string.Empty;
}
