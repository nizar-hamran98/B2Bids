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
public class PermissionsController(IMediator _mediator) : ControllerBase
{
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost("CreateRolePermission")]
    [AuthorizePermission(UserAccessPermission.CreatePermissions)]
    public async Task<ActionResult> CreateRolePermission([FromBody] RolePermissionModel model)
    {
        var result = await _mediator.Send(new CreateRolePermissionRequest { RolePermission = model });
        return result;
    }


    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpGet("GetRolePermissions/{roleId}")]
    [AuthorizePermission(UserAccessPermission.ReadPermissions)]
    public async Task<ActionResult> GetRolePermissions(long roleId)
    {
        var result = await _mediator.Send(new GetRolePermissionsRequest { RoleId = roleId });
        return result;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.ReadPermissions)]
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetPermissionsRequest());
        return result;
    }
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.ReadPermissions)]
    public async Task<ActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetPermissionRequest { PermissionId = id });
        return result;
    }
}