using CookWizard.Application.Features.Recipes.Commands;
using CookWizard.Domain.Entities;
using CookWizard.Domain.Interfaces.Repository;
using FluentAssertions;
using Moq;

namespace CookWizard.Tests.Application.Recipes;

public class CreateRecipeHandlerTests
{
    private readonly Mock<IRecipeRepository> _repositoryMock;
    private readonly CreateRecipeHandler _handler;

    public CreateRecipeHandlerTests()
    {
        _repositoryMock = new Mock<IRecipeRepository>();
        _handler = new CreateRecipeHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Should_Create_Recipe_Successfully()
    {
        // Arrange
        var command = new CreateRecipeCommand(
            "Test Recipe",
            2,
            1200,
            new List<SectionDTO>
            {
                new SectionDTO(
                    "Main",
                    new List<IngredientDTO>
                    {
                        new IngredientDTO(1, "kg", "chicken", null, null)
                    },
                    new List<PreparationStepDTO>
                    {
                        new PreparationStepDTO("Cook it")
                    }
                )
            }
        );

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Recipe>()))
            .ReturnsAsync("recipe-id");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().Be(false);
        result.Value.Should().Be("recipe-id");

        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Recipe>()), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_When_Portions_Is_Invalid()
    {
        var command = new CreateRecipeCommand(
            "Test",
            0,
            100,
            new List<SectionDTO>()
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("porciones");
    }
}