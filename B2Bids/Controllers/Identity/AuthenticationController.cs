using Identity.Application.RequestHandler;
using Identity.Domain.Models;
using Kernel.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SharedKernel;

namespace Identity.API.Controllers;

[Route("api/Identity/[controller]")]
[ApiController]
public class AuthenticationController(IMediator _mediator, IHttpContext _httpContext) : ControllerBase
{
    [Consumes("application/json")]
    [HttpPost("GetToken")]
    public async Task<ActionResult> GetToken([FromBody] TokenModel model, CancellationToken cancellationToken)
    {
        AuthenticationModel AuthModel = new() { UserName = model.UserName, Password = model.Password };
        var response = await _mediator.Send(new AuthenticateUser(AuthModel, Request, true, false), cancellationToken);
        return response;
    }

    [Consumes("application/json")]
    [HttpPost]
    public async Task<ActionResult> Authenticate([FromBody] AuthenticationModel model, CancellationToken cancellationToken) => await _mediator.Send(new AuthenticateUser(model, Request, false, true), cancellationToken);

    [Consumes("application/json")]
    [Authorize(AuthenticationSchemes = "AzureAd")]
    [HttpPost("AuthenticateWithAzure")]
    public async Task<ActionResult> AuthenticateWithAzure(CancellationToken cancellationToken)
    {
        if (User.Identity.IsAuthenticated)
        {
           var userDTOResult = await _mediator.Send(new AuthenticateUserWithAzure { User = User, Request = Request }, cancellationToken);
           return userDTOResult;
           
        }
        return Result.Failure("Not Authenticated");
    }
    [Consumes("application/json")]
    [HttpGet("RefreshToken")]
    public async Task<ActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        StringValues token = _httpContext.Current.Request.Headers["X-Authorization-Token"];
        StringValues refreshToken = _httpContext.Current.Request.Headers["X-Refresh-Token"];
        string? RemoteIPAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

        var response = await _mediator.Send(new RefreshToken(token, refreshToken, RemoteIPAddress), cancellationToken);
        return response;
    }
    [Consumes("application/json")]
    [HttpGet("SignOut")]
    public async Task<ActionResult> SignOut(CancellationToken cancellationToken)
    {
        StringValues accessToken = _httpContext.Current.Request.Headers["Authorization"];
        StringValues remoteIPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

        var response = await _mediator.Send(new SignOut(accessToken, remoteIPAddress), cancellationToken);
        return response;
    }
    [Consumes("application/json")]
    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword(string email, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ResetPassword(email), cancellationToken);
        return response;
    }
    [Consumes("application/json")]
    [HttpPost("ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePassword changePassword)
    {
        var response = await _mediator.Send(changePassword);
        return response;
    }
    [Consumes("application/json")]
    [HttpPost("ValidateUserToken")]
    public async Task<ActionResult> ValidateUserToken(string token, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new ValidateUserToken(token), cancellationToken);

        return response;
    }
    [Consumes("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("GenerateToken")]
    public async Task<ActionResult> GenerateToken([FromBody] AuthenticationModel model, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GenerateTokenRequest(model, Request), cancellationToken);
        return Result.Success(response);
    }
}