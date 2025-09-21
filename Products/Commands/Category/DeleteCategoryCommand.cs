using Products.Domain.Entities;
using SharedKernel;

namespace Products.Application.Commands;
public class DeleteCategoryCommand : ICommand
{
    public required Category Category { get; set; }
}

public class DeleteCategoryCommandHandler(IRepositoryBase<Category> repo, IUnitOfWork unitOfWork) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
    {
        await repo.DeleteAsync(command.Category, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}
