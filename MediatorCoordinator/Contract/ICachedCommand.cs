using MediatR;

namespace MediatorCoordinator.Contract;
public interface ICachedCommand<out TResponse> : IRequest<TResponse>, ICachedCommand { }
public interface ICachedCommand
{  
}