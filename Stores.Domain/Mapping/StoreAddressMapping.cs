using SharedKernel;
using Stores.Domain.Entities;
using Stores.Domain.Models;

namespace Stores.Domain.Mapping;
public static class StoreAddressMapping
{
    public static IEnumerable<StoreAddressModel> ToModels(this IEnumerable<StoreAddress> entities) => entities.Select(x => x.ToModel());
    public static StoreAddressModel? ToModel(this StoreAddress entity)
    {
        return entity == null
            ? null
            : new StoreAddressModel
            {
                Id = entity.Id,
                Country = entity.Country,
                City = entity.City,
                Address = entity.Address,
                Longitude = entity.Longitude,
                Latitude = entity.Latitude,
                Status = (EntityStatus)entity.StatusId,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy
                
            };
    }

    public static StoreAddress ToEntity(this StoreAddressModel model)
    {
        return new StoreAddress
        {
            Id = model.Id,
            Country = model.Country,
            City = model.City,
            Address = model.Address,
            Longitude = model.Longitude,
            Latitude = model.Latitude,
            StatusId = (short)model.Status,
        };
    }

    public static StoreAddress? ToUpdatedEntity(this StoreAddress entity, StoreAddressModel model)
    {
        if (entity is null) return null;

        entity.City = string.IsNullOrEmpty(model.City) ? entity.City : model.City.Trim();
        entity.Country = string.IsNullOrEmpty(model.Country) ? entity.Country : model.Country.Trim();
        entity.Longitude = string.IsNullOrEmpty(model.Longitude) ? entity.Longitude : model.Longitude.Trim();
        entity.Latitude = string.IsNullOrEmpty(model.Latitude) ? entity.Latitude : model.Latitude.Trim();
        entity.StatusId = (short)model.Status > 0 ? (short)model.Status : entity.StatusId;

        return entity;
    }
}
