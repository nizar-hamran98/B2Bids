using SharedKernel;

namespace Products.Application.Commands;
public class AddUpdateProductCommand : ICommand
{
    public required Domain.Entities.Product Product { get; set; }
}

public class AddUpdateProductCommandHandler(IRepositoryBase<Domain.Entities.Product> repo, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateProductCommand>
{
    public async Task Handle(AddUpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Product.Id > 0)
            await repo.UpdateAsync(command.Product);
        else
            await repo.AddAsync(command.Product);

        await unitOfWork.SaveChanges(cancellationToken);
    }
}
