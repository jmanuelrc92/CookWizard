using CookWizard.Application.Recipes.UseCases.SearchRecipes;
using CookWizard.Domain.Recipes.Models;
using CookWizard.Domain.Recipes.Repository;
using FluentAssertions;
using Moq;

namespace CookWizard.Tests.Application.Recipes;

public class SearchRecipesHandlerTests
{
    private readonly Mock<IRecipeRepository> _repositoryMock;
    private readonly SearchRecipesHandler _handler;

    public SearchRecipesHandlerTests()
    {
        _repositoryMock = new Mock<IRecipeRepository>();
        _handler = new SearchRecipesHandler();
    }

    [Fact]
    public async Task Should_Search_Recipes_With_Default_Pagination_And_Sorting()
    {
        _repositoryMock
            .Setup(r => r.SearchAsync(It.IsAny<RecipeSearchCriteria>()))
            .ReturnsAsync(new RecipeSearchResult(
                new List<Recipe>
                {
                    new Recipe("Pasta", 2, 1200)
                },
                1
            ));

        var result = await _handler.Handle(
            new SearchRecipesQuery(null, null),
            _repositoryMock.Object
        );

        result.Page.Should().Be(1);
        result.PageSize.Should().Be(20);
        result.TotalItems.Should().Be(1);
        result.TotalPages.Should().Be(1);
        result.Items.Should().ContainSingle();
        result.Items.First().Name.Should().Be("Pasta");

        _repositoryMock.Verify(r => r.SearchAsync(It.Is<RecipeSearchCriteria>(criteria =>
            criteria.SearchText == null &&
            criteria.MinimumCookingTimeInSeconds == null &&
            criteria.MaximumCookingTimeInSeconds == null &&
            criteria.SortBy == RecipeSearchSortBy.Newest &&
            criteria.Skip == 0 &&
            criteria.Limit == 20
        )), Times.Once);
    }

    [Fact]
    public async Task Should_Trim_Search_Text_And_Map_Medium_Cooking_Time_Filter()
    {
        _repositoryMock
            .Setup(r => r.SearchAsync(It.IsAny<RecipeSearchCriteria>()))
            .ReturnsAsync(new RecipeSearchResult(new List<Recipe>(), 0));

        await _handler.Handle(
            new SearchRecipesQuery(
                new PaginationRequest(2, 10),
                new RecipeSearchFilter("  soup  ", RecipeCookingTimeFilter.Medium),
                RecipeSortBy.NameAscending
            ),
            _repositoryMock.Object
        );

        _repositoryMock.Verify(r => r.SearchAsync(It.Is<RecipeSearchCriteria>(criteria =>
            criteria.SearchText == "soup" &&
            criteria.MinimumCookingTimeInSeconds == 31 * 60 &&
            criteria.MaximumCookingTimeInSeconds == 90 * 60 &&
            criteria.SortBy == RecipeSearchSortBy.NameAscending &&
            criteria.Skip == 10 &&
            criteria.Limit == 10
        )), Times.Once);
    }

    [Fact]
    public async Task Should_Reject_Invalid_Page_Size()
    {
        var action = async () => await _handler.Handle(
            new SearchRecipesQuery(new PaginationRequest(1, 101), null),
            _repositoryMock.Object
        );

        await action.Should().ThrowAsync<ArgumentException>()
            .WithMessage("PageSize must be greater than 0 and less than or equal to 100.");
    }
}
