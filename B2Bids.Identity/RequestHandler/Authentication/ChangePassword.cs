using Identity.Application.Commands;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using Identity.Domain.Mapping;
using Identity.Domain.Validators;
using Kernel.Contract;
using Kernel.Helpers;
using MediatorCoordinator.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Identity.Application.RequestHandler;

public class ChangePasswordResponse
{
    public bool Status { get; set; }
}

public class ChangePassword : IRequestContext<Result>
{
    public string? token { get; set; }
    public long? userId { get; set; }
    public string? oldPassword { get; set; }
    public string? newPassword { get; set; }
    public string? confirmedPassword { get; set; }
    public required ControllerContext context;
}

public class ChangePasswordHandler(Dispatcher dispatcher, IJwtManager jwtManager, IHttpContext httpContext) : IRequestHandler<ChangePassword, Result>
{
    public async Task<Result> Handle(ChangePassword data, CancellationToken cancellationToken)
    {
        string userId = string.IsNullOrEmpty(data.token) ? httpContext.UserId : jwtManager.ParseTokenV2(data.token).FirstOrDefault().Value;
        User user = await dispatcher.DispatchAsync(new GetUserQuery { Id = long.Parse(userId), AsNoTracking = true });

        Domain.Models.UserModel userObj = user.ToModel();
        if (userObj == null)
            return Result.Failure("InValid UserName or Password");

        if (!string.IsNullOrEmpty(data.oldPassword))
        {
            string oldPassHashed = HashPassword.Hash(data.oldPassword);
            if (oldPassHashed != user.PasswordHash)
                return Result.Failure("Old Password Incorrect");
        }
        else if (string.IsNullOrEmpty(data.token))
            return Result.Failure("Old Password is Required");

        if (data.newPassword != data.confirmedPassword)
            return Result.Failure("Password are not matching");

        if (!PasswordCheck.IsValidPassword(data.newPassword, 8, 1, false, true, true, true))
            return Result.Failure("Password Does Not Meet Minimum Security Requirements");

        user.PasswordHash = HashPassword.Hash(data.newPassword);
        await dispatcher.DispatchAsync(new AddUpdateUserCommand { User = user });
        return Result.Success("Successfully Changed Password");
    }
}