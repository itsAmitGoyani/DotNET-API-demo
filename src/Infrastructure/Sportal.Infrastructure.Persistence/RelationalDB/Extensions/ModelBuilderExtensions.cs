using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sportal.Domain.Entities;
using Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations;

namespace Sportal.Infrastructure.Persistence.RelationalDB.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        MethodInfo configureMethod = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods
            .Single(m => m.Name == nameof(BaseEntityConfiguration.Configure));

        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsSubclassOf(typeof(BaseEntity)))
            {
                configureMethod.MakeGenericMethod(entityType.ClrType).Invoke(null, new[] { modelBuilder });
            }
        }

        return modelBuilder;
    }

    public static ModelBuilder ApplySoftDeleteEntityConfiguration(this ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        MethodInfo configureMethod = typeof(SoftDeletableEntityConfiguration).GetTypeInfo().DeclaredMethods
            .Single(m => m.Name == nameof(SoftDeletableEntityConfiguration.Configure));

        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsSubclassOf(typeof(SoftDeletableEntity)))
            {
                configureMethod.MakeGenericMethod(entityType.ClrType).Invoke(null, new[] { modelBuilder });
            }
        }

        return modelBuilder;
    }
}