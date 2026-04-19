namespace CookWizard.Domain.Entities;
public class Recipe
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public int Portions { get; private set; }
    public int TotalTimeInSeconds { get; private set; }
    public Difficulty Difficulty { get; private set; }
    private readonly List<Section> _sections = new();
    public IReadOnlyCollection<Section> Sections => _sections;
    
    private Recipe() { } // Mongo

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