namespace LocalsService.Interfaces.REST.Resources;

public record CreateReportResource(int LocalId, string Title, int UserId, string Description);