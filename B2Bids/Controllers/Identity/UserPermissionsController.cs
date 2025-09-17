using Identity.Application.RequestHandler;
using Identity.Domain.Constants;
using Identity.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Identity.API.Controllers;

[Route("api/Identity/[controller]")]
[ApiController]
[AuthorizePermission]
public class UserPermissionsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [AuthorizePermission(UserAccessPermission.ReadUserPermissions)]
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetUserPermissionsRequest());
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpPost("CreateUserPermission")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [AuthorizePermission(UserAccessPermission.CreateUserPermissions)]
    public async Task<ActionResult> CreateUserPermission([FromBody] UserPermissionModel model)
    {
        var result = await _mediator.Send(new CreateUserPermissionRequest { Model = model });
        return result;
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.ReadUserPermissions)]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetUserPermissionRequest { Id = id });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("GetUserPermissions/{userId}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [AuthorizePermission(UserAccessPermission.ReadUserPermissions)]
    public async Task<ActionResult> GetUserPermissions(long userId)
    {
        var result = await _mediator.Send(new GetUserPermissionsRequest { UserId = userId });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.RemoveUserPermissions)]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteUserPermissionRequest { UserPermissionId = id });
        return result;
    }
}