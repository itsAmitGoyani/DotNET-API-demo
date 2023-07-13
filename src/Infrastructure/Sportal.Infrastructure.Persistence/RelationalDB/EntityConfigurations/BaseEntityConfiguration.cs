using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sportal.Domain.Entities;

namespace Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations;

public class BaseEntityConfiguration
{
    public static void Configure<TEntity>(ModelBuilder modelBuilder)
        where TEntity : BaseEntity
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        modelBuilder.Entity<TEntity>(builder =>
        {
            builder.HasOne(be => be.CreatedByUser).WithMany().HasForeignKey(be => be.CreatedBy).IsRequired();
            builder.Property(cr => cr.CreatedAtUtc).IsRequired().Metadata
                .SetAfterSaveBehavior(PropertySaveBehavior.Throw);

            builder.HasOne(be => be.UpdatedByUser).WithMany().HasForeignKey(be => be.UpdatedBy).IsRequired(false);
            builder.Property(cr => cr.UpdatedAtUtc).IsRequired(false).Metadata
                .SetBeforeSaveBehavior(PropertySaveBehavior.Throw);
        });
    }
}