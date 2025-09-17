namespace MediatorCoordinator.Contract
{
    public interface IServiceTransactionCoordinator
    {
        Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> serviceOperation, CancellationToken cancellationToken = default);
    }
}
