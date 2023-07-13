using System.ComponentModel.DataAnnotations;

namespace Sportal.Models;

public class EntryRequestModel
{
    [Required]
    public Guid DepartmentId { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; } = null!;
    
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Gender { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
    
    [Required]
    public string CNIC { get; set; } = null!;
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public DateTime CardIssue { get; set; }

    [Required]
    public DateTime CardExpiry { get; set; }
    
    public int? Childern { get; set; }
    
    public string? Vahicle { get; set; } = null!;
    
    [Required]
    public Guid EntryBy { get; set; }
}