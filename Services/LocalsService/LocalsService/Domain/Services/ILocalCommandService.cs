using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Commands;

namespace LocalsService.Domain.Services;

public interface ILocalCommandService
{
    Task<Local?> Handle(CreateLocalCommand command);
    Task<Local?> Handle(UpdateLocalCommand command);
}