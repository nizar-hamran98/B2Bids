using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Stores.Application.RequestHandlers;
using Stores.Domain.Models;

namespace B2Bids.APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
//[AuthorizePermission]
public class StoreController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetStoreRequest());
        return result?.Count() == 0 ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get(long id)
    {
        var result = await _mediator.Send(new GetStoreByRequest { Id = id });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("GetByName/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> GetByName(string name)
    {
        var result = await _mediator.Send(new GetStoreByRequest { Name = name });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //[AuthorizePermission(UserAccessPermission.CreateUsers)]
    public async Task<ActionResult> Post([FromBody] StoreModel model) => await _mediator.Send(new AddUpdateStoreRequest { Model = model });

    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ModifyUsers)]
    public async Task<ActionResult> Put(long id, [FromBody] StoreModel model) => await _mediator.Send(new AddUpdateStoreRequest { Id = id, Model = model });

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.RemoveUsers)]
    public async Task<ActionResult> Delete(int id) => await _mediator.Send(new DeleteStoreRequest { Id = id });
}
