using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Mapping;
using Identity.Domain.Models;
using Identity.Domain.Validators;
using Kernel.Helpers;
using MediatorCoordinator.Contract;
using MediatR;
using SharedKernel;

public class PostUserRequest : IRequestContext<Result<UserModel>>
{
    public required UserModel Model { get; set; }
}

public class PostUserRequestHandler(Dispatcher _dispatcher) : IRequestHandler<PostUserRequest, Result<UserModel>>
{
    public async Task<Result<UserModel>> Handle(PostUserRequest request, CancellationToken cancellationToken)
    {
        
        var userNameResult = await _dispatcher.DispatchAsync(new GetUserQuery { UserName = request.Model.UserName });
        if (userNameResult != null)
            return Result.Failure("User Name Already Exists");

        var emailResult = await _dispatcher.DispatchAsync(new GetUserQuery { Email = request.Model.Email });
        if (emailResult != null)
            return Result.Failure("Email Already Exists");

        
        var userEntity = request.Model.ToEntity();
        if (!PasswordCheck.IsValidPassword(userEntity.PasswordHash, 8, 1, true, true, true, true))
        {
            return Result.Failure("Weak Password");
        }

        userEntity.PasswordHash = HashPassword.Hash(userEntity.PasswordHash);


        await _dispatcher.DispatchAsync(new AddUpdateUserCommand { User = userEntity });

        var newModel = userEntity.ToModel();

        return Result.Success(userEntity.ToModel());
    }
}
