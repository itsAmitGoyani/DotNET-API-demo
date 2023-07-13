using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportal.Domain.Entities.DepartmentAggregate;

namespace Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations.DepartmentAggregate;

public class VisitorConfiguration : IEntityTypeConfiguration<Visitor>
{
    public void Configure(EntityTypeBuilder<Visitor> builder)
    {
        builder.ToTable("Visitors");
                
        builder.HasKey(au => au.Id);
    }
}