using Microsoft.AspNetCore.Identity;

namespace Sportal.Domain.Entities.UsersAggregate;

public class ApplicationUser: IdentityUser<Guid>
{
    public string CNIC { get; set; } = null!;
}