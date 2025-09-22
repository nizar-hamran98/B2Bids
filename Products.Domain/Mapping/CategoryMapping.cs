using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Domain.Mapping;
public static class CategoryMapping
{
    public static IEnumerable<CategoryModel> ToModels(this IEnumerable<Category> entities) => entities.Select(x => x.ToModel());

    public static CategoryModel? ToModel(this Category entity)
    {
        return entity == null
            ? null
            : new CategoryModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                CreatedBy = entity.CreatedBy,
                UpdatedAt = entity.UpdatedAt,
                UpdatedBy = entity.UpdatedBy
            };
    }

    public static Category ToEntity(this CategoryModel model)
    {
        return new Category
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            StatusId = (short)model.Status,
        };
    }

    public static Category? ToUpdatedEntity(this Category entity, CategoryModel model)
    {
        if (entity is null) return null;

        entity.Name = string.IsNullOrEmpty(model.Name) ? entity.Name : model.Name.Trim();
        entity.Description = string.IsNullOrEmpty(model.Description) ? entity.Description : model.Description.Trim();
        entity.StatusId = (short)model.Status > 0 ? (short)model.Status : entity.StatusId;

        return entity;
    }
}
