namespace Sportal.Domain.Entities.DepartmentAggregate;

public class Visit
{
    public Guid Id { get; set; }
    
    public Guid DepartmentId { get; set; }
    
    public Guid VisitorId { get; set; }

    public string PhoneNumber { get; set; } = null!;
    
    public Guid EntryBy { get; set; }
    
    public DateTime EntryAtUtc { get; set; }
    
    public Guid? ExitBy { get; set; }
    
    public DateTime? ExitAtUtc { get; set; }
    
    public Visitor Visitor { get; set; }
    
    public Department Department { get; set; }
}