using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportal.Domain.Entities.DepartmentAggregate;

namespace Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations.DepartmentAggregate;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(au => au.Id);
    }
}