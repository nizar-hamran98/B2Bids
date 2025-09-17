using SharedKernel;

namespace MediatorCoordinator;

public class MediatorCoordinatorOptions
{
    public  Func<IServiceProvider, IUnitOfWork> UnitOfWorkFactory { get; set; }
}