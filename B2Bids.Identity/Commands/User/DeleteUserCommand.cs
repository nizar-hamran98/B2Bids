using Identity.Domain.Entities;
using SharedKernel;

namespace Identity.Application.Commands;

public class DeleteUserCommand : ICommand
{
    public User User { get; set; }
}

public class DeleteUserCommandHandler(IRepositoryBase<User> _repository, IUnitOfWork _unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.User, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}