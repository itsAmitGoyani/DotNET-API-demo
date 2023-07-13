using System.ComponentModel.DataAnnotations;

namespace Sportal.Models;

public class EntryResponseModel
{
    [Required]
    public string ReceiptId { get; set; } = null!;
}