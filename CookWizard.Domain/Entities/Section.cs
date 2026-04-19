namespace CookWizard.Domain.Entities;

public class Section
{
    public string Name { get; private set; }

    private readonly List<Ingredient> _ingredients = new();
    public IReadOnlyCollection<Ingredient> Ingredients => this._ingredients;

    private readonly List<PreparationStep> _steps = new();
    public IReadOnlyCollection<PreparationStep> Steps => this._steps;

    private Section() { }

    public Section(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Section name is required");

        this.Name = name;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
            throw new ArgumentNullException(nameof(ingredient));

        if (this._ingredients.Any(i => i.Product == ingredient.Product && i.Unit == ingredient.Unit))
            throw new InvalidOperationException("Ingredient already exists in section");

        this._ingredients.Add(ingredient);
    }

    public void AddStep(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Step description is required");

        int order = this._steps.Count + 1;
        var step = new PreparationStep(order, description);

        this._steps.Add(step);
    }
}