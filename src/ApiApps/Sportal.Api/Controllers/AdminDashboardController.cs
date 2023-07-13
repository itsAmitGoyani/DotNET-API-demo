using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using Sportal.Application;
using Sportal.Application.Services;
using Sportal.Models;

namespace Sportal.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("v1/admin")]
[Authorize(Roles = "Admin")]
public class AdminDashboardController : ApiController
{
    private readonly AdminDashboardService _adminDashboardService;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="adminDashboardService"></param>
    public AdminDashboardController(AdminDashboardService adminDashboardService)
    {
        _adminDashboardService = adminDashboardService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="departmentId"></param>
    /// <returns></returns>
    [HttpGet("activity-log/{departmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ActivityLogResponseModel>> GetActivityLog(Guid departmentId)
    {
        try
        {
            var activityLogResponseModel =
                await _adminDashboardService.GetActivityLogAsync(departmentId);
            return Ok(activityLogResponseModel);
        }
        catch (NotFoundException exception)
        {
            return NotFound(new ErrorResponse(ApiErrors.NotFound, exception.Message, StatusCodes.Status404NotFound));
        }
        catch (BadRequestException exception)
        {
            return BadRequest(new ErrorResponse(ApiErrors.BadRequest, exception.Message,
                StatusCodes.Status400BadRequest));
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}