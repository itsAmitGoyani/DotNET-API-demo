using System.ComponentModel.DataAnnotations;

namespace Sportal.Models;

public class ExitResponseModel
{
    [Required]
    public string status { get; set; } 
}