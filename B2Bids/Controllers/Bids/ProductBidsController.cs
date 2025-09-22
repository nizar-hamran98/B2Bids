using Bid.Application.RequestHandlers;
using Bid.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace B2Bids.APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
//[AuthorizePermission]
public class ProductBidsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetProductBidsRequest());
        return result?.Count() == 0 ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get(long id)
    {
        var result = await _mediator.Send(new GetProductBidsByRequest { Id = id });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("GetByProductId/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> GetByProductId(long productId)
    {
        var result = await _mediator.Send(new GetProductBidsByRequest { ProductId = productId });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("GetByWinnerId/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> GetByWinnerId(long winnerId)
    {
        var result = await _mediator.Send(new GetProductBidsByRequest { WinnerId = winnerId });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //[AuthorizePermission(UserAccessPermission.CreateUsers)]
    public async Task<ActionResult> Post([FromBody] ProductBidsModel model) => await _mediator.Send(new AddUpdateProductBidsRequest { Model = model });

    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ModifyUsers)]
    public async Task<ActionResult> Put(long id, [FromBody] ProductBidsModel model) => await _mediator.Send(new AddUpdateProductBidsRequest { Id = id, Model = model });

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.RemoveUsers)]
    public async Task<ActionResult> Delete(int id) => await _mediator.Send(new DeleteProductBidsRequest { Id = id });
}
