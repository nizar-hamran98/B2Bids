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
public class RolesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [AuthorizePermission(UserAccessPermission.ReadRoles)]
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetRolesRequest());
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.ReadRoles)]
    public async Task<ActionResult> Get(long id)
    {
        var result = await _mediator.Send(new GetRoleRequest { Id = id });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("GetActiveRoles")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [AuthorizePermission(UserAccessPermission.ReadRoles)]
    public async Task<ActionResult> GetActiveRoles()
    {
        var result = await _mediator.Send(new GetRolesRequest { Status = EntityStatus.Active });
        return result == null ? Result.NotFound() : Result.Success(result);

    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [AuthorizePermission(UserAccessPermission.CreateRoles)]
    public async Task<ActionResult> Post([FromBody] RoleModel model)
    {
        var result = await _mediator.Send(new PostRoleRequest { Model = model });
        return result;
    }

    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.ModifyRoles)]
    public async Task<Result<RoleModel>> Put(long id, [FromBody] RoleModel model)
    {
        Result<RoleModel> result = await _mediator.Send(new PutRoleRequest { RoleId = id, Model = model });
        return result;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.RemoveRoles)]
    public async Task<ActionResult> Delete(long id)
    {
        var result = await _mediator.Send(new DeleteRoleRequest { RoleId = id });
        return result;
    }

}
