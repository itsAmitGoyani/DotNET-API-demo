using Sportal.Domain.Entities.UsersAggregate;

namespace Sportal.Domain.Entities;

public class BaseEntity
{
    public Guid CreatedBy { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public ApplicationUser CreatedByUser { get; set; } = null!;

    public ApplicationUser? UpdatedByUser { get; set; }
}