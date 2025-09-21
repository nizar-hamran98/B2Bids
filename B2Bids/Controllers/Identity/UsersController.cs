using Identity.Application.RequestHandler;
using Identity.Domain.Constants;
using Identity.Domain.Models;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace B2Bids.Controllers;

[Route("api/Identity/[controller]")]
[ApiController]
//[AuthorizePermission]
public class UsersController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetUsersRequest());
        return result?.Count() == 0 ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get(long id)
    {
        var result = await _mediator.Send(new GetUserRequest { Id = id });
        return result != null ? Result.NotFound() : Result.Success(result);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //[AuthorizePermission(UserAccessPermission.CreateUsers)]
    public async Task<ActionResult> Post([FromBody] UserModel model) => await _mediator.Send(new PostUserRequest { Model = model });

    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ModifyUsers)]
    public async Task<ActionResult> Put(long id, [FromBody] UserUpdateModel model) => await _mediator.Send(new PutUserRequest { UserId = id, Model = model });

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizePermission(UserAccessPermission.RemoveUsers)]
    public async Task<ActionResult> Delete(int id) => await _mediator.Send(new DeleteUserRequest { Id = id });

    [HttpPost("ResetNumberOfLoginRetries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ModifyUsers)]
    public async Task<ActionResult> ResetNumberOfRetries(long id) => await _mediator.Send(new ResetNumberOfRetriesRequest { UserId = id });
}
