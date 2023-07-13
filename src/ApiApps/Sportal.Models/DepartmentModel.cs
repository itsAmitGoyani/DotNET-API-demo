using System.ComponentModel.DataAnnotations;

namespace Sportal.Models;

public class DepartmentModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null;
}