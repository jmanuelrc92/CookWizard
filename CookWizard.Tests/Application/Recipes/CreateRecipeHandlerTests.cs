using CookWizard.Application.Recipes.UseCases.CreateRecipe;
using CookWizard.Domain.Recipes.Models;
using CookWizard.Domain.Recipes.Repository;
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
        _handler = new CreateRecipeHandler();
    }

    [Fact]
    public async Task Should_Create_Recipe_Successfully()
    {
        // Arrange
        var command = new CreateRecipeCommand(
            "Test Recipe",
            2,
            1200,
            new List<CreateSection>
            {
                new CreateSection(
                    "Main",
                    new List<CreateIngredient>
                    {
                        new CreateIngredient(1, "kg", "chicken", null, null)
                    },
                    new List<CreateStep>
                    {
                        new CreateStep("Cook it")
                    }
                )
            }
        );

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Recipe>()))
            .ReturnsAsync("recipe-id");

        // Act
        var result = await _handler.Handle(command, _repositoryMock.Object);

        // Assert
        result.Should().Be(false);
        result.Should().Be("recipe-id");

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Recipe>()), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_When_Portions_Is_Invalid()
    {
        var command = new CreateRecipeCommand(
            "Test",
            0,
            100,
            new List<CreateSection>()
        );

        var result = await _handler.Handle(command, _repositoryMock.Object);

        result.Should().Be(false);
    }
}