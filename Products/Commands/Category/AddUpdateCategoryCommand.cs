using Products.Domain.Entities;
using SharedKernel;

namespace Products.Application.Commands;
public class AddUpdateCategoryCommand : ICommand
{
    public required Category Category { get; set; }
}

public class AddUpdateCategoryCommandHandler(IRepositoryBase<Category> repo, IUnitOfWork unitOfWork) : ICommandHandler<AddUpdateCategoryCommand>
{
    public  async Task Handle(AddUpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (command.Category.Id > 0)
                await repo.UpdateAsync(command.Category);
            else
                await repo.AddAsync(command.Category);

            await unitOfWork.SaveChanges(cancellationToken);
        }
        catch(Exception ex)
        {
            var test = "";
        }
    }
}
