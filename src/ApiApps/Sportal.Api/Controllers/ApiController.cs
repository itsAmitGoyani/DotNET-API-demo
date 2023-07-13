using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sportal.Models;

namespace Sportal.Api.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    ///     Gets get logged in user ID.
    /// </summary>
    //protected Guid LoggedInUserId => User.GetLoggedInUserId();

    /// <summary>
    ///     Bad request object result.
    /// </summary>
    /// <param name="modelState">Model state dictionary.</param>
    /// <returns>Bad request if successful.</returns>
    public override BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState)
    {
        return BadRequest(new ErrorResponse(ApiErrors.BadRequest, "Bad Request", StatusCodes.Status400BadRequest,
            modelState));
    }

    /// <summary>
    ///     Get status code of API errors.
    /// </summary>
    /// <param name="statusCode">Action result status code.</param>
    /// <param name="message">Model message.</param>
    /// <returns>Status code.</returns>
    public ActionResult StatusCode([ActionResultStatusCode] int statusCode, string message)
    {
        return StatusCode(statusCode, new ErrorResponse(ApiErrors.InternalServerError, message, statusCode));
    }
}