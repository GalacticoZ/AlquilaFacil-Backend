using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Repositories;
using LocalsService.Locals.Domain.Services;
using Shared.Application.External.OutboundServices;
using Shared.Domain.Repositories;

namespace LocalsService.Application.Internal.CommandServices;

public class CommentCommandService(ICommentRepository commentRepository, ILocalRepository localRepository, IUserExternalService userExternalService, IUnitOfWork unitOfWork) : ICommentCommandService
{
    public async Task<Comment?> Handle(CreateCommentCommand command)
    {
        var local = await localRepository.FindByIdAsync(command.LocalId);

        if (local == null)
        {
            throw new KeyNotFoundException("There is no locals matching the id specified");
        }
        
        var userExists = await userExternalService.UserExists(command.UserId);
        
        if (!userExists)
        {
            throw new KeyNotFoundException("There are no users matching the id specified");
        }
        
        if (command.Rating is > 5 or < 0)
        {
            throw new KeyNotFoundException("Rating needs to be a number between 0 and 5");
        }

        var comment = new Comment(command);
        await commentRepository.AddAsync(comment);
        await unitOfWork.CompleteAsync();
        return comment;
    }
}