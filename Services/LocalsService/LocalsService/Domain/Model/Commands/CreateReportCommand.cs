namespace LocalsService.Domain.Model.Commands;

public record CreateReportCommand(int LocalId, string Title, int UserId, string Description);