using IAMService.Domain.Model.Aggregates;

namespace IAMService.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
    Task<int?> ValidateToken(string token);
}