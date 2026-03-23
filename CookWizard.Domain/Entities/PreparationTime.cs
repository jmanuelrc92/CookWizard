namespace CookWizard.Domain.Entities;

public class PreparationTime
{
    public int Time { get; set; }
    public string Unit { get; set; } = "minutes";
    public int TimeInSeconds { get; set; }
}
