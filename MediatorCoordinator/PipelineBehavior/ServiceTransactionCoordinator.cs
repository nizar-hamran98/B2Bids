using MediatorCoordinator.Contract;
using SharedKernel;

namespace MediatorCoordinator
{
    internal sealed class ServiceTransactionCoordinator(IUnitOfWork unitOfWork) : IServiceTransactionCoordinator
    {
        public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> serviceOperation, CancellationToken cancellationToken = default)
        {
            try
            {
                unitOfWork.BeginTransaction();
                TResult result = await serviceOperation();

                if (result != null && ((Result<dynamic>)result).IsSuccess)
                    await unitOfWork.Commit(cancellationToken);
                else
                    await unitOfWork.Rollback(cancellationToken);

                return result;
            }
            catch (Exception)
            {
                await unitOfWork.Rollback(cancellationToken);
                throw;
            }
        }
    }

}
