using Bid.Application.RequestHandlers;
using Bid.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace B2Bids.APIs.Controllers;

[Route("api/[controller]")]
[ApiController]
//[AuthorizePermission]
public class BiddingController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get()
    {
        var result = await _mediator.Send(new GetBiddingRequest());
        return result?.Count() == 0 ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> Get(long id)
    {
        var result = await _mediator.Send(new GetBiddingByRequest { Id = id });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("GetByProductId/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> GetByProductId(long productId)
    {
        var result = await _mediator.Send(new GetBiddingByRequest { ProductId = productId });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpGet("GetBidderId/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ReadUsers)]
    public async Task<ActionResult> GetBidderId(long bidderId)
    {
        var result = await _mediator.Send(new GetBiddingByRequest { BidderId = bidderId });
        return result == null ? Result.NotFound() : Result.Success(result);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    //[AuthorizePermission(UserAccessPermission.CreateUsers)]
    public async Task<ActionResult> Post([FromBody] BiddingModel model) => await _mediator.Send(new AddUpdateBiddingRequest { Model = model });

    [HttpPut("{id}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.ModifyUsers)]
    public async Task<ActionResult> Put(long id, [FromBody] BiddingModel model) => await _mediator.Send(new AddUpdateBiddingRequest { Id = id, Model = model });

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    //[AuthorizePermission(UserAccessPermission.RemoveUsers)]
    public async Task<ActionResult> Delete(int id) => await _mediator.Send(new DeleteBiddingRequest { Id = id });
}
