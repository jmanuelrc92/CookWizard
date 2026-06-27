namespace CookWizard.Domain.Recipes.Models;

public class Recipe
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Portions { get; set; }
    public int TotalTimeInSeconds { get; set; }
    public Difficulty Difficulty { get; set; }
    private List<Section> _sections = new();
    public IReadOnlyCollection<Section> Sections => _sections;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Recipe() { }

    public Recipe(string name, int portions, int totalTimeInSeconds, Difficulty difficulty)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if (portions <= 0)
            throw new ArgumentException("Portions must be greater than 0");

        this.Id = Guid.NewGuid().ToString();
        this.Name = name;
        this.Portions = portions;
        this.Difficulty = difficulty;
        this.TotalTimeInSeconds = totalTimeInSeconds;
    }

    public void AddSection(Section section)
    {
        if (section == null)
            throw new ArgumentNullException(nameof(section));

        if (_sections.Any(s => s.Name == section.Name))
            throw new InvalidOperationException("Section already exists");

        this._sections.Add(section);
    }
}
