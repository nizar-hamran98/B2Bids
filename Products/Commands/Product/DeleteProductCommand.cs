using SharedKernel;

namespace Products.Application.Commands;
public class DeleteProductCommand : ICommand
{
    public required Domain.Entities.Product Product { get; set; }
}

public class DeleteProductCommandHandler(IRepositoryBase<Domain.Entities.Product> repo, IUnitOfWork unitOfWork) : ICommandHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken = default)
    {
        await repo.DeleteAsync(command.Product, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
    }
}
