namespace CookWizard.Domain.Entities;

public class PreparationTime
{
    public int Value { get; set; }
    public string Unit { get; set; } = "minutes";
    public int Seconds { get; set; }
}
