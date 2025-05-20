using SubscriptionsService.Locals.Domain.Model.Aggregates;
using SubscriptionsService.Locals.Domain.Model.Commands;
using SubscriptionsService.Locals.Domain.Model.Queries;
using SubscriptionsService.Locals.Domain.Services;
using SubscriptionsService.Locals.Interfaces.REST;
using SubscriptionsService.Locals.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace SubscriptionsService.Tests.CoreIntegrationTests;

public class LocalControllerTests
{
    [Fact]
    public async Task CreateLocal_ReturnsCreatedLocal_WhenSuccess()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        var resource = new CreateLocalResource(
            District: "Miraflores",
            Street: "Av. Larco 123",
            LocalName: "Sala Ejecutiva Premium",
            Country: "Perú",
            City: "Lima",
            Price: 250,
            PhotoUrl: "https://example.com/photo.jpg",
            DescriptionMessage: "Sala con proyector y aire acondicionado",
            LocalCategoryId: 2,
            UserId: 10,
            Features: "WiFi, Proyector, Aire Acondicionado",
            Capacity: 30
        );

        var expectedLocal = new Local(
            resource.District,
            resource.Street,
            resource.LocalName,
            resource.Country,
            resource.City,
            resource.Price,
            resource.PhotoUrl,
            resource.DescriptionMessage,
            resource.LocalCategoryId,
            resource.UserId,
            resource.Features,
            resource.Capacity
        )
        {
            Id = 456 // Simulación de que ya fue creado con ID
        };

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateLocalCommand>()))
            .ReturnsAsync(expectedLocal);

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.CreateLocal(resource);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var localResource = Assert.IsType<LocalResource>(createdResult.Value);

        Assert.Equal(456, localResource.Id);
        Assert.Equal("Sala Ejecutiva Premium", localResource.LocalName);
        Assert.Equal(250, localResource.NightPrice);
        Assert.Equal(30, localResource.Capacity);
        Assert.Equal(10, localResource.UserId);
        Assert.Equal("https://example.com/photo.jpg", localResource.PhotoUrl);
    }
    
    [Fact]
    public async Task GetAllLocals_ReturnsAllLocals()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        var locals = new List<Local>
        {
            new("Miraflores", "Av. Larco", "Sala A", "Perú", "Lima", 200, "urlA", "Desc A", 1, 1, "WiFi", 20),
            new("San Isidro", "Calle B", "Sala B", "Perú", "Lima", 250, "urlB", "Desc B", 2, 2, "WiFi AC", 30)
        };

        mockQueryService
            .Setup(s => s.Handle(It.IsAny<GetAllLocalsQuery>()))
            .ReturnsAsync(locals);

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetAllLocals();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedLocals = Assert.IsAssignableFrom<IEnumerable<LocalResource>>(okResult.Value);
        Assert.Equal(2, returnedLocals.Count());
    }

    [Fact]
    public async Task GetLocalById_ReturnsLocal_WhenExists()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        var local = new Local("Miraflores", "Av. Larco", "Sala A", "Perú", "Lima", 200, "urlA", "Desc A", 1, 1, "WiFi", 20)
        {
            Id = 10
        };

        mockQueryService
            .Setup(s => s.Handle(It.Is<GetLocalByIdQuery>(q => q.LocalId == 10)))
            .ReturnsAsync(local);

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetLocalById(10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var localResource = Assert.IsType<LocalResource>(okResult.Value);
        Assert.Equal(10, localResource.Id);
    }

    [Fact]
    public async Task GetLocalById_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        mockQueryService
            .Setup(s => s.Handle(It.IsAny<GetLocalByIdQuery>()))
            .ReturnsAsync((Local?)null);

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetLocalById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateLocal_ReturnsUpdatedLocal_WhenSuccess()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        var updateResource = new UpdateLocalResource(
            "San Isidro",
            "Calle Reforma",
            "Sala Actualizada",
            "Perú",
            "Lima",
            300,
            "urlNew",
            "Actualizada",
            3,
            1,
            "WiFi, Proyector",
            40
        );

        var updatedLocal = new Local(updateResource.District, updateResource.Street, updateResource.LocalName, updateResource.Country,
            updateResource.City, updateResource.Price, updateResource.PhotoUrl, updateResource.DescriptionMessage,
            updateResource.LocalCategoryId, 1, updateResource.Features, updateResource.Capacity)
        {
            Id = 789
        };

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<UpdateLocalCommand>()))
            .ReturnsAsync(updatedLocal);

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.UpdateLocal(789, updateResource);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resource = Assert.IsType<LocalResource>(okResult.Value);
        Assert.Equal(789, resource.Id);
        Assert.Equal("Sala Actualizada", resource.LocalName);
    }

    [Fact]
    public async Task GetUserLocals_ReturnsLocals_WhenExists()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        var userId = 5;
        var locals = new List<Local>
        {
            new("San Miguel", "Calle Uno", "Local A", "Perú", "Lima", 150, "urlA", "Desc A", 1, userId, "WiFi", 15),
            new("Pueblo Libre", "Calle Dos", "Local B", "Perú", "Lima", 180, "urlB", "Desc B", 2, userId, "WiFi, AC", 25)
        };

        mockQueryService
            .Setup(s => s.Handle(It.Is<GetLocalsByUserIdQuery>(q => q.UserId == userId)))
            .ReturnsAsync(locals);

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.GetUserLocals(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var localResources = Assert.IsAssignableFrom<IEnumerable<LocalResource>>(okResult.Value);
        Assert.Equal(2, localResources.Count());
    }

    [Fact]
    public async Task SearchByCategoryIdAndCapacityRange_ReturnsMatchingLocals()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        int categoryId = 2, min = 10, max = 50;

        var locals = new List<Local>
        {
            new("Barranco", "Calle Arte", "Estudio A", "Perú", "Lima", 220, "url", "Arte y cultura", categoryId, 1, "Proyector", 20)
        };

        mockQueryService
            .Setup(s => s.Handle(It.Is<GetLocalsByCategoryIdAndCapacityRangeQuery>(q =>
                q.LocalCategoryId == categoryId && q.MinCapacity == min && q.MaxCapacity == max)))
            .ReturnsAsync(locals);

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result = await controller.SearchByCategoryIdAndCapacityRange(categoryId, min, max);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var localResources = Assert.IsAssignableFrom<IEnumerable<LocalResource>>(okResult.Value);
        Assert.Single(localResources);
    }

    [Fact]
    public async Task GetAllDistricts_ReturnsDistricts()
    {
        // Arrange
        var mockCommandService = new Mock<ILocalCommandService>();
        var mockQueryService = new Mock<ILocalQueryService>();

        var expectedDistricts = new HashSet<string> { "District1", "District2" };
        
        mockQueryService
            .Setup(s => s.Handle(It.IsAny<GetAllLocalDistrictsQuery>()))
            .Returns(expectedDistricts);  // Devuelve directamente el HashSet

        var controller = new LocalsController(mockCommandService.Object, mockQueryService.Object);

        // Act
        var result =  controller.GetAllDistricts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);  // Verifica que es un OkObjectResult
        var returnedDistricts = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);  // Verifica que es IEnumerable
        Assert.Equal(expectedDistricts.Count, returnedDistricts.Count());  // Verifica la cantidad de elementos
    }
}