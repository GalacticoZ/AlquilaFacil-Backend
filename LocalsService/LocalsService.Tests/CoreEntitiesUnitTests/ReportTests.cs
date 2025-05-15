using LocalsService.Locals.Domain.Model.Aggregates;
using LocalsService.Locals.Domain.Model.Commands;

namespace LocalsService.Tests.CoreEntitiesUnitTests;

public class ReportTests
{
    [Fact]
    public void Report_Constructor_WithCommand_ShouldInitializeProperties()
    {
        // Arrange
        var command = new CreateReportCommand(1, "Test Report", 1, "This is a test report.");

        // Act
        var report = new Report(command);

        // Assert
        Assert.Equal(command.LocalId, report.LocalId);
        Assert.Equal(command.Title, report.Title);
        Assert.Equal(command.UserId, report.UserId);
        Assert.Equal(command.Description, report.Description);
    }
}