
using System.ComponentModel.DataAnnotations;

namespace Sportal.Models;

/// <summary>
/// 
/// </summary>
public class ActivityLogResponseModel
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int MaleInn { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int FemaleInn { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int ChildrenInn { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int VehicleInn { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int MaleOut { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int FemaleOut { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int ChildrenOut { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public int VehicleOut { get; set; }
}