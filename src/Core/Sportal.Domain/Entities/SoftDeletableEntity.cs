using Sportal.Domain.Entities.UsersAggregate;

namespace Sportal.Domain.Entities;

public class SoftDeletableEntity : BaseEntity
{
    public Guid? DeletedBy { get; set; }

    public DateTime? DeletedAtUtc { get; set; }

    public ApplicationUser? DeletedByUser { get; set; }
}