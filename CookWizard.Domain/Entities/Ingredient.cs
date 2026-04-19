namespace CookWizard.Domain.Entities;

public class Ingredient
{
    public decimal Quantity { get; private set; }
    public Unit? Unit { get; private set; }
    public string Product { get; private set; }
    public string? Notes { get; private set; }
    public string? Raw { get; private set; }

    private Ingredient() { }

    public Ingredient(decimal quantity, string product, Unit unit)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product is required");

        Quantity = quantity;
        Unit = unit;
        Product = product.Trim().ToLower();
    }

    public Ingredient(decimal quantity, string product, Unit unit, string raw)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product is required");

        Quantity = quantity;
        Unit = unit;
        Product = product.Trim().ToLower();
        Raw = raw.Trim();
    }

    public Ingredient(decimal quantity, string product)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");
        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product is required");
        Quantity = quantity;
        Product = product.Trim().ToLower();
    }

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

    public void AddRaw(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return;
        Raw = raw.Trim();
    }
}