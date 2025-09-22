using Bid.Domain.Entities;
using Bid.Domain.Models;
using SharedKernel;

namespace Bid.Domain.Mapping;
public static class BiddingMapping
{
    public static IEnumerable<BiddingModel> ToModels(this IEnumerable<Bidding> entities) => entities.Select(x => x.ToModel());
    public static BiddingModel? ToModel(this Bidding entity)
    {
        return entity == null
            ? null
            : new BiddingModel
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                BidPrice = entity.BidPrice,
                BidderId = entity.BidderId,
                Status = (EntityStatus)entity.StatusId,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy

            };
    }

    public static Bidding ToEntity(this BiddingModel model)
    {
        return new Bidding
        {
            Id = model.Id,
            ProductId = model.ProductId,
            BidPrice = model.BidPrice,
            BidderId = model.BidderId,
            StatusId = (short)model.Status,
        };
    }

    public static Bidding? ToUpdatedEntity(this Bidding entity, BiddingModel model)
    {
        if (entity is null) return null;

        entity.ProductId = (short)model.ProductId > 0 ? (short)model.ProductId : entity.ProductId;
        entity.BidPrice = (short)model.BidPrice > 0 ? (short)model.BidPrice : entity.BidPrice;
        entity.BidderId = (short)model.BidderId > 0 ? (short)model.BidderId : entity.BidderId;
        entity.StatusId = (short)model.Status > 0 ? (short)model.Status : entity.StatusId;

        return entity;
    }
}
