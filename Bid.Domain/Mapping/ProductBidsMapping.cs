using Bid.Domain.Entities;
using Bid.Domain.Models;
using SharedKernel;

namespace Bid.Domain.Mapping;
public static class ProductBidsMapping
{
    public static IEnumerable<ProductBidsModel> ToModels(this IEnumerable<ProductBids> entities) => entities.Select(x => x.ToModel());
    public static ProductBidsModel? ToModel(this ProductBids entity)
    {
        return entity == null
            ? null
            : new ProductBidsModel
            {
                Id = entity.Id,
                InitialBid = entity.InitialBid,
                LastBid = entity.LastBid,
                WinnerId = entity.WinnerId,
                TotalOfBids = entity.TotalOfBids,
                EndBidDate = entity.EndBidDate,
                ListOfBidders = entity.ListOfBidders,
                Status = (EntityStatus)entity.StatusId,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy

            };
    }

    public static ProductBids ToEntity(this ProductBidsModel model)
    {
        return new ProductBids
        {
            Id = model.Id,
            InitialBid = model.InitialBid,
            LastBid = model.LastBid,
            WinnerId = model.WinnerId,
            TotalOfBids = model.TotalOfBids,
            EndBidDate = model.EndBidDate,
            ListOfBidders = model.ListOfBidders,
            StatusId = (short)model.Status,
        };
    }

    public static ProductBids? ToUpdatedEntity(this ProductBids entity, ProductBidsModel model)
    {
        if (entity is null) return null;

        entity.InitialBid = model.InitialBid;
        entity.LastBid = model.LastBid;
        entity.WinnerId = model.WinnerId;
        entity.TotalOfBids = model.TotalOfBids;
        entity.EndBidDate = model.EndBidDate;
        entity.ListOfBidders = model.ListOfBidders;
        entity.StatusId = (short)model.Status > 0 ? (short)model.Status : entity.StatusId;

        return entity;
    }
}
