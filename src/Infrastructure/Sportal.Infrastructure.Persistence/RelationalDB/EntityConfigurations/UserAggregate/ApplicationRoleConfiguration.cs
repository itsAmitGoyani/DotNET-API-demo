using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sportal.Domain.Entities.UsersAggregate;

namespace Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations.UserAggregate;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        
    }
}