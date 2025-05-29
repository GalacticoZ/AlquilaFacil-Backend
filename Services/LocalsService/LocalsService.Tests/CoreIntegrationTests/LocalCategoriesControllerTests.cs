using LocalsService.Domain.Model.Entities;
using Moq;
using LocalsService.Locals.Interfaces.REST;
using LocalsService.Domain.Services;
using LocalsService.Domain.Model.Queries;
using LocalsService.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;

namespace LocalsService.Tests.CoreIntegrationTests;

public class LocalCategoriesControllerTests
{
    [Fact]
    public async Task GetAllLocalCategories_ReturnsOkResult_WithLocalCategoryResources()
    {
        // Arrange
        var mockQueryService = new Mock<ILocalCategoryQueryService>();
        
        var expectedCategories = new List<LocalCategory>
        {
            new LocalCategory { Id = 1, Name = "Category1" },
            new LocalCategory { Id = 2, Name = "Category2" }
        };
        
        mockQueryService
            .Setup(s => s.Handle(It.IsAny<GetAllLocalCategoriesQuery>()))
            .ReturnsAsync(expectedCategories);

        var controller = new LocalCategoriesController(mockQueryService.Object);

        // Act
        var result = await controller.GetAllLocalCategories();  // Llamada asincrónica al controlador

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<LocalCategoryResource>>(okResult.Value); 
        Assert.Equal(expectedCategories.Count, returnedCategories.Count());
    }
}