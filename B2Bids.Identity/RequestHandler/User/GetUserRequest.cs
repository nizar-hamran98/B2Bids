using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatR;
using SharedKernel;

namespace Identity.Application.RequestHandler;
public class GetUserRequest : IRequest<UserModel>
{
    public long Id { get; set; }
}

internal sealed class GetUserRequestHandler(Dispatcher dispatcher) : IRequestHandler<GetUserRequest, UserModel>
{
    public async Task<UserModel> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var userResult = await dispatcher.DispatchAsync(new GetUserQuery
        {
            Id = (long)request.Id,
            AsNoTracking = true
        });

        return userResult.ToModel();
    }
}
