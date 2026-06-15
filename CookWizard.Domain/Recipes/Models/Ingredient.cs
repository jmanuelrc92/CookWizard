namespace CookWizard.Domain.Recipes.Models;

public class Ingredient
{
    public decimal Quantity { get; private set; }
    public Unit? Unit { get; private set; }
    public string Product { get; private set; }
    public string? Notes { get; private set; }
    public string? Raw { get; private set; }

    private Ingredient() { }

    public Ingredient(decimal quantity, string product, string raw)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product is required");

        Quantity = quantity;
        Product = product.Trim().ToLower();
        Raw = raw.Trim();
    }

    public void AddNotes(string notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
            return;

        Notes = notes.Trim().ToLower();
    }

}