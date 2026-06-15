namespace CookWizard.Domain.Recipes.Models;

public class PreparationStep
{
    public int Order { get; private set; }
    public string Description { get; private set; }

    private PreparationStep() { }

    internal PreparationStep(int order, string description)
    {
        if (order <= 0)
            throw new ArgumentException("Order must be greater than 0");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");

        this.Order = order;
        this.Description = description.Trim().ToLower();
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required");

        this.Description = description.Trim().ToLower();
    }
}