using Products.Domain.Models;

namespace Products.Domain.Mapping;
public static class ProductMapping
{
    public static IEnumerable<ProductModel> ToModels(this IEnumerable<Entities.Product> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static ProductModel? ToModel(this Entities.Product entity)
    {
        return entity == null
            ? null
            : new ProductModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                ExpiresIn = entity.ExpiresIn,
                Description = entity.Description,
                Images = entity.Images, 
                CategoryId = entity.CategoryId,
                StoreId = entity.StoreId,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy
            };
    }

    public static Entities.Product ToEntity(this ProductModel model)
    {
        return new Entities.Product
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,    
            Description = model.Description,
            ExpiresIn= model.ExpiresIn,
            Images= model.Images,
            CategoryId= model.CategoryId,
            StoreId= model.StoreId,
            StatusId = (short)model.Status,
        };
    }

    public static Entities.Product? ToUpdatedEntity(this Entities.Product entity, ProductModel model)
    {
        if (entity is null) return null;

        entity.Name = string.IsNullOrEmpty(model.Name) ? entity.Name : model.Name.Trim();
        entity.Description = string.IsNullOrEmpty(model.Description) ? entity.Description : model.Description.Trim();
        entity.Price = model.Price;
        entity.ExpiresIn = model.ExpiresIn;
        entity.Images = model.Images.Count > 0 ? model.Images : entity.Images;
        entity.StatusId = (short)model.Status > 0 ? (short)model.Status : entity.StatusId;

        return entity;
    }
}
