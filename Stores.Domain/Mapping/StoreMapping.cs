using SharedKernel;
using Stores.Domain.Entities;
using Stores.Domain.Models;

namespace Stores.Domain.Mapping;
public static class StoreMapping
{
    public static IEnumerable<StoreModel> ToModels(this IEnumerable<Store> entities) => entities.Select(x => x.ToModel());
    public static StoreModel? ToModel(this Store entity)
    {
        return entity == null
            ? null
            : new StoreModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Logo = entity.Logo,
                StoreRegNo = entity.StoreRegNo,
                StoreRegNoImage = entity.StoreRegNoImage,
                IsAuthenticated = (bool?)entity.IsAuthenticated,
                StoreAddressId = entity.StoreAddressId,
                StoreAddress = entity.StoreAddress,
                Status = (EntityStatus)entity.StatusId,
                StoreType = entity.StoreType,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy,
            };
    }

    public static Store ToEntity(this StoreModel model)
    {
        return new Store
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Logo = model.Logo,
            StoreRegNo = model.StoreRegNo,
            StoreRegNoImage = model.StoreRegNoImage,
            IsAuthenticated = model.IsAuthenticated,
            StoreType = model.StoreType,
            StoreAddressId = model.StoreAddressId,
            StoreAddress = model.StoreAddress,
        };
    }

    public static Store? ToUpdatedEntity(this Store entity, StoreModel model)
    {
        if (entity is null) return null;

        entity.Name = string.IsNullOrEmpty(model.Name) ? entity.Name : model.Name.Trim();
        entity.Description = string.IsNullOrEmpty(model.Description) ? entity.Description : model.Description.Trim();
        entity.StoreRegNo = string.IsNullOrEmpty(model.StoreRegNo) ? entity.StoreRegNo : model.StoreRegNo.Trim();
        entity.IsAuthenticated = model.IsAuthenticated;
        entity.StoreType = model.StoreType;
        entity.Logo = model.Logo;
        entity.StoreRegNoImage = model.StoreRegNoImage;
        entity.StoreAddressId = model.StoreAddressId;
        entity.StatusId = (short)model.Status > 0 ? (short)model.Status : entity.StatusId;

        return entity;
    }
}
