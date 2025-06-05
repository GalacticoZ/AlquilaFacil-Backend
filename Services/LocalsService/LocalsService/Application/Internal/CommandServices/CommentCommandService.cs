using LocalsService.Application.External.OutboundServices;
using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Repositories;
using LocalsService.Locals.Domain.Services;
using Shared.Domain.Repositories;

namespace LocalsService.Application.Internal.CommandServices;

public class CommentCommandService(ICommentRepository commentRepository, ILocalRepository localRepository, IUserCommentExternalService userCommentExternalService, IUnitOfWork unitOfWork) : ICommentCommandService
{
    public async Task<Comment?> Handle(CreateCommentCommand command)
    {
        var local = await localRepository.FindByIdAsync(command.LocalId);
        var isUserExists = await userCommentExternalService.UserExists(command.UserId);

        if (local == null)
        {
            throw new Exception("There is no locals matching the id specified");
        }
        
        if (!isUserExists)
        {
            throw new Exception("There are no users matching the id specified");
        }
        
        if (command.Rating is > 5 or < 0)
        {
            throw new Exception("Rating needs to be a number between 0 and 5");
        }

        var comment = new Comment(command);
        await commentRepository.AddAsync(comment);
        await unitOfWork.CompleteAsync();
        return comment;
    }
}