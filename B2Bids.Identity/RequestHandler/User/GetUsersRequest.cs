using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;
public class GetUsersRequest : IRequest<IEnumerable<UserModel>>
{
}

public class GetUsersRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetUsersRequest, IEnumerable<UserModel>>
{
    public async Task<IEnumerable<UserModel>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        var Users = await dispatcher.DispatchAsync(new GetUsersQuery(), cancellationToken);
        return Users.ToModels();
    }
}
