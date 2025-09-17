using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SharedKernel;
public static class EntityTypeBuilderExtensions
{
    public static DataBuilder<TEntity> SeedDataWithUniqueLongId<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, [NotNull] params object[] data)
           where TEntity : class
    {
        return entityTypeBuilder.HasData(data.Select(obj => _CopyObjectWithLongId<TEntity>(obj)));
    }

    private static T _CopyObjectWithLongId<T>(object obj)
    {
        var copy = _CopyObject<T>(obj);
        return copy;
    }
    private static T _CopyObject<T>(object source)
    {
        var instance = (T)Activator.CreateInstance(typeof(T), nonPublic: true);
        return _CopyObject(instance, source);
    }
    private static T _CopyObject<T>(T instance, object source)
    {
        var sourceType = source.GetType();
        var destType = typeof(T);

        foreach (var sourceProp in sourceType.GetProperties())
        {
            var destProp = destType.GetProperty(sourceProp.Name);
            if (destProp == null) continue;

            var value = sourceProp.GetValue(source);

            if (destProp.SetMethod != null)
            {
                destProp.SetValue(instance, value);
            }
            else
            {
                var backingField = destType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(f => f.Name.StartsWith($"<{sourceProp.Name}>"));
                if (backingField != null)
                {
                    backingField.SetValue(instance, value);
                }
            }
        }

        return instance;
    }

}
