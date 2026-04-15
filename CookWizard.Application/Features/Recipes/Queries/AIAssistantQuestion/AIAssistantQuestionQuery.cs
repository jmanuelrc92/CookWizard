using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;

namespace CookWizard.Application.Features.Recipes.Queries;

// La petición: una simple pregunta en texto
public record AIAssistantQuestionQuery(string question) : IRequestCustom<CookWizardApiResult<string>>;

public class AIAssistantQuestionHandler : IRequestHandlerCustom<AIAssistantQuestionQuery, CookWizardApiResult<string>>
{
    private readonly IRecipeRepository _repository;

    public AIAssistantQuestionHandler(IRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<CookWizardApiResult<string>> HandleAsync(AIAssistantQuestionQuery request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.question))
            return CookWizardApiResult<string>.Failure("La pregunta no puede estar vacía.");

        // --- Lógica del Asistente ---
        // 1. (IA / Lógica simple) Extraemos los ingredientes clave de la pregunta.
        //    Ejemplo: "Tengo mango y fresas..." -> ["mango", "fresa"]
        var keyIngredients = ExtractIngredientsFromText(request.question);

        if (!keyIngredients.Any())
            return CookWizardApiResult<string>.Success("¡Hola! ¿Qué ingredientes tienes o qué tipo de comida te gustaría preparar?");

        // 2. Usamos nuestra Query existente para buscar en MongoDB.
        var recipes = await _repository.SearchByIngredientAsync(keyIngredients);

        // 3. Formateamos la respuesta del Asistente (esto es lo que dirá la IA).
        var formatedAnswer = FormatAnswer(keyIngredients, recipes);

        return CookWizardApiResult<string>.Success(formatedAnswer);
    }

    // --- Lógica de Ayuda (Helper Methods) ---
    private List<string> ExtractIngredientsFromText(string text)
    {
        // Esta es una implementación básica. 
        // ¡Aquí es donde OpenAI brillará después!
        text = text.ToLower();
        var dictionary = new List<string> { "mango", "pollo", "fresa", "papas", "leche" };

        return dictionary.Where(i => text.Contains(i)).ToList();
    }

    private string FormatAnswer(List<string> ingredients, IEnumerable<Recipe> recipes)
    {
        if (!recipes.Any())
        {
            return $"¡Vaya! No encontré ninguna receta que combine: {string.Join(", ", ingredients)}. " +
                   $"¿Te gustaría probar con otros ingredientes?";
        }

        var titles = recipes.Select(r => r.Name).ToList();
        return $"¡Genial! Con {string.Join(" y ", ingredients)} puedes cocinar: " +
               $"{string.Join(", ", titles)}. Toca una para ver los pasos.";
    }
}