using System.ComponentModel.DataAnnotations;

namespace Sportal.Models;

public class ExitRequestModel
{
    [Required]
    public string ReceiptId { get; set; } = null!;
    
    [Required]
    public Guid ExitBy { get; set; }
}