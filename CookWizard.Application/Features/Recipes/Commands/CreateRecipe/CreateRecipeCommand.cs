using CookWizard.Application.Common;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces;
using MediatR;

namespace CookWizard.Application.Features.Recipes.Commands;

public record CreateRecipeCommand(
    string Name,
    int Portions,
    int TotalTimeInSeconds,
    List<SectionDTO> Sections
) : IRequest<ResultObject<string>>;
public record IngredientDTO(
    decimal Quantity,
    string Unit,
    string Product,
    string? Notes,
    string? Raw
);
public record PreparationStepDTO(string Description);
public record SectionDTO(
    string Name,
    List<IngredientDTO> Ingredients,
    List<PreparationStepDTO> Steps
);

public class CreateRecipeHandler : IRequestHandler<CreateRecipeCommand, ResultObject<string>>
{
    private readonly IRecipeRepository _recipeRepository;

    public CreateRecipeHandler(IRecipeRepository recipeRepository)
    {
        this._recipeRepository = recipeRepository;
    }

    public async Task<ResultObject<string>> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        // --- Validaciones ---
        if (string.IsNullOrWhiteSpace(request.Name))
            return ResultObject<string>.Failure("El nombre es requerido");

        if (request.Portions <= 0)
            return ResultObject<string>.Failure("Las porciones deben ser mayores a cero");

        if (request.TotalTimeInSeconds < 0)
            return ResultObject<string>.Failure("El tiempo no puede ser negativo");

        if (request.Sections == null || !request.Sections.Any())
            return ResultObject<string>.Failure("Debe haber al menos una sección");

        if (request.Sections.Any(s => s.Ingredients == null || !s.Ingredients.Any()))
            return ResultObject<string>.Failure("Cada sección debe tener ingredientes");

        // --- Crear entidad usando dominio ---
        var recipe = new Recipe(
            request.Name,
            request.Portions,
            request.TotalTimeInSeconds,
            Difficulty.Easy // puedes mapear luego
        );

        foreach (var sectionDto in request.Sections)
        {
            var section = new Section(sectionDto.Name);

            // Ingredientes
            foreach (var ing in sectionDto.Ingredients)
            {
                var ingredient = new Ingredient(
                    ing.Quantity,
                    ing.Product,
                    Domain.Entities.Unit.Gram // Aquí deberías mapear el string a tu enum de unidades
                );

                if (!string.IsNullOrWhiteSpace(ing.Notes))
                    ingredient.AddNotes(ing.Notes);

                if (!string.IsNullOrWhiteSpace(ing.Raw))
                    ingredient.AddRaw(ing.Raw);

                section.AddIngredient(ingredient);
            }

            // Steps (sin order)
            foreach (var stepDto in sectionDto.Steps)
            {
                section.AddStep(stepDto.Description);
            }

            recipe.AddSection(section);
        }

        // --- Persistencia ---
        var id = await _recipeRepository.CreateAsync(recipe);

        return ResultObject<string>.Success(id);
    }
}