using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportal.Domain.Entities.UsersAggregate;

namespace Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations.UserAggregate;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(au => au.Id);
    }
}