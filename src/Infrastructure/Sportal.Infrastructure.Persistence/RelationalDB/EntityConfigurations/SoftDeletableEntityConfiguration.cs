using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sportal.Domain.Entities;

namespace Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations;

public class SoftDeletableEntityConfiguration
{
    public static void Configure<TEntity>(ModelBuilder modelBuilder)
        where TEntity : SoftDeletableEntity
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.Entity<TEntity>(builder =>
        {
            builder.HasOne(be => be.DeletedByUser).WithMany().HasForeignKey(be => be.DeletedBy).IsRequired(false);
            builder.Property(cr => cr.DeletedAtUtc).IsRequired(false).Metadata
                .SetBeforeSaveBehavior(PropertySaveBehavior.Throw);
            //builder.HasQueryFilter(m => EF.Property<DateTime?>(m, "DeletedAtUtc") == null);
        });
    }
}