namespace Sportal.Domain.Entities.DepartmentAggregate;

public class Visitor
{
    public Guid Id { get; set; }

    public string PhoneNumber { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string Gender { get; set; } = null!;
    
    public string Address { get; set; } = null!;
    
    public string CNIC { get; set; } = null!;
    
    public DateTime DateOfBirth { get; set; }
    
    public DateTime CardIssue { get; set; }

    public DateTime CardExpiry { get; set; }
    
    public int? Childern { get; set; }
    
    public string? Vahicle { get; set; } = null!;
}