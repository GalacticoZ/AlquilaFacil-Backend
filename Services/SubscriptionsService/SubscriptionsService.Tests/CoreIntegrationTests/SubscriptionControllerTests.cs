using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Interfaces.REST;
using SubscriptionsService.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SubscriptionsService.Domain.Model.Commands;

namespace SubscriptionsService.Tests.CoreIntegrationTests;

public class SubscriptionControllerTests
{
    [Fact]
    public async Task CreateSubscription_ReturnsCreatedWithSubscription()
    {
        var mockCommandService = new Mock<ISubscriptionCommandService>();
        var mockQueryService = new Mock<ISubscriptionQueryServices>();

        var command = new CreateSubscriptionCommand(1, 1, "voucherUrl");
        var subscription = new Subscription(command);

        mockCommandService
            .Setup(s => s.Handle(It.IsAny<CreateSubscriptionCommand>()))
            .ReturnsAsync(subscription);

        var controller = new SubscriptionsController(mockCommandService.Object, mockQueryService.Object);

        var resource = new CreateSubscriptionResource(command.PlanId, command.UserId, command.VoucherImageUrl);

        var result = await controller.CreateSubscription(resource);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
    }
    
    [Fact]
    public async Task GetAllSubscriptions_ReturnsOkWithList()
    {
        var mockCommandService = new Mock<ISubscriptionCommandService>();
        var mockQueryService = new Mock<ISubscriptionQueryServices>();

        var subscriptions = new List<Subscription>
        {
            new Subscription(new CreateSubscriptionCommand(1, 1, "voucherUrl1")),
            new Subscription(new CreateSubscriptionCommand(2, 2, "voucherUrl2")),
            new Subscription(new CreateSubscriptionCommand(3, 3, "voucherUrl3"))
        };

        mockQueryService
            .Setup(q => q.Handle(It.IsAny<GetAllSubscriptionsQuery>()))
            .ReturnsAsync(subscriptions);

        var controller = new SubscriptionsController(mockCommandService.Object, mockQueryService.Object);

        var result = await controller.GetAllSubscriptions();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
    
    [Fact]
    public async Task ActiveSubscriptionStatus_ReturnsOkIfSuccess()
    {
        var mockCommandService = new Mock<ISubscriptionCommandService>();
        var mockQueryService = new Mock<ISubscriptionQueryServices>();

        var subscription = new Subscription(new CreateSubscriptionCommand(1, 1, "voucherUrl1"));
        subscription.Id = 1;

        mockCommandService
            .Setup(c => c.Handle(It.Is<ActiveSubscriptionStatusCommand>(cmd => cmd.SubscriptionId == 1)))
            .ReturnsAsync(subscription);

        var controller = new SubscriptionsController(mockCommandService.Object, mockQueryService.Object);

        var result = await controller.ActiveSubscriptionStatus(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    
}