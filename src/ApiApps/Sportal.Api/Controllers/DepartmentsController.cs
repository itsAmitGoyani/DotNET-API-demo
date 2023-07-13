using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using Sportal.Application.Services;
using Sportal.Models;

namespace Sportal.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("v1/departments")]
[Authorize(Roles = "SuperAdmin")]
public class DepartmentController : ApiController
{
    private readonly DepartmentService _departmentService;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="departmentService"></param>
    public DepartmentController(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<DepartmentModel>>> GetDepartments()
    {
        try
        {
            List<DepartmentModel> departmentModels =await _departmentService.GetDepartmentsAsync();
            return Ok(departmentModels);
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("entry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<EntryResponseModel>> Entry([Required] EntryRequestModel model)
    {
        try
        {
            EntryResponseModel entryResponseModel = await _departmentService.EntryAsync(model);
            return Ok(entryResponseModel);
        } catch (NotFoundException exception)
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("exit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ExitResponseModel>> Entry([Required] ExitRequestModel model)
    {
        try
        {
            ExitResponseModel entryResponseModel = await _departmentService.ExitAsync(model);
            return Ok(entryResponseModel);
        } catch (NotFoundException exception)
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