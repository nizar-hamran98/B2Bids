using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

public class PutUserRequest : IRequestContext<Result<UserModel>>
{
    public long UserId { get; set; }
    public required UserUpdateModel Model { get; set; }
}

public class PutUserRequestHandler(Dispatcher dispatcher) : IRequestHandler<PutUserRequest, Result<UserModel>>
{
    public async Task<Result<UserModel>> Handle(PutUserRequest request, CancellationToken cancellationToken)
    {
        var userNameResult = await dispatcher.DispatchAsync(new GetUsersQuery { AsNoTracking = true, UserName = request.Model.UserName.Trim() });
        if (userNameResult.Any(user => user.Id != request.UserId))
        {
            return Result.Failure("User Name Already Exists");
        }

        var emailResult = await dispatcher.DispatchAsync(new GetUsersQuery { AsNoTracking = true, Email = request.Model.Email.Trim() });
        if (emailResult.Any(user => user.Id != request.UserId))
        {
            return Result.Failure("Email Already Exists");
        }

        var existingUserResult = await dispatcher.DispatchAsync(new GetUserQuery { Id = request.UserId });
        if (existingUserResult == null)
        {
            return Result.NotFound("User not found");
        }

        var updatedEntity = existingUserResult.ToUpdatedEntity(request.Model);


        await dispatcher.DispatchAsync(new AddUpdateUserCommand { User = updatedEntity! });
        var newModel = updatedEntity!.ToModel();

        return Result.Success(newModel!);
    }
}