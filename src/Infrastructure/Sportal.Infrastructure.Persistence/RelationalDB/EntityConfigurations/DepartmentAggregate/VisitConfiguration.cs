using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportal.Domain.Entities.DepartmentAggregate;

namespace Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations.DepartmentAggregate;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ToTable("Visits");
        
        builder.HasKey(au => au.Id);
    }
}