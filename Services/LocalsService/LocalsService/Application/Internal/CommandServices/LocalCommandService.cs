using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Repositories;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Services;
using Shared.Application.External.OutboundServices;
using Shared.Domain.Repositories;

namespace LocalsService.Application.Internal.CommandServices;

public class LocalCommandService (ILocalRepository localRepository, ILocalCategoryRepository localCategoryRepository, IUserExternalService userExternalService, IUnitOfWork unitOfWork) : ILocalCommandService
{
    
    public async Task<Local?> Handle(CreateLocalCommand command)
    {
        var localCategory = await localCategoryRepository.FindByIdAsync(command.LocalCategoryId);
        if (localCategory == null)
        {
            throw new KeyNotFoundException("Local category not found");
        }
        
        var userExists = await userExternalService.UserExists(command.UserId);
        
        if (!userExists)
        {
            throw new KeyNotFoundException("There are no users matching the id specified");
        } 

        if (command.Price <= 0)
        {
            throw new BadHttpRequestException("Price must be greater than 0");
        }
        var local = new Local(command);
        await localRepository.AddAsync(local);
        await unitOfWork.CompleteAsync();
        return local;
    }

    public async Task<Local?> Handle(UpdateLocalCommand command)
    {
        var local = await localRepository.FindByIdAsync(command.Id);
        if (local == null)
        {
            throw new KeyNotFoundException("Local not found");
        }
        var localCategory = await localCategoryRepository.FindByIdAsync(command.LocalCategoryId);
        if (localCategory == null)
        {
            throw new KeyNotFoundException("Local category not found");
        }

        if (command.Price <= 0)
        {
            throw new Exception("Price must be greater than 0");
        }
        localRepository.Update(local);
        local.Update(command);
        await unitOfWork.CompleteAsync();
        return local;
    }
}