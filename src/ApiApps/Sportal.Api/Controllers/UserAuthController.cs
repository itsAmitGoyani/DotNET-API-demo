using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Sportal.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sportal.Models;
using SendGrid.Helpers.Errors.Model;
using Sportal.Domain.Entities.UsersAggregate;

namespace Sportal.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("v1/auth")]
public class UserAuthController : ApiController
{
    private readonly UserService _userService;
    private readonly IConfiguration _config;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="config"></param>
    public UserAuthController(UserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<LoginResponse>> LoginWithUserNameAndPassword([Required] LoginRequestModel model)
    {
        try
        {
            var issuer = _config.GetSection("Jwt:Issuer").Value;
            var key = _config.GetSection("Jwt:Key").Value;

            if (issuer == null)
            {
                return BadRequest(new ErrorResponse(ApiErrors.BadRequest,
                    "Invalid jwt issuer.", StatusCodes.Status400BadRequest));
            }

            if (key == null)
            {
                return BadRequest(new ErrorResponse(ApiErrors.BadRequest,
                    "Invalid jwt key.", StatusCodes.Status400BadRequest));
            }

            var applicationUser = await _userService.GetUserInfoByUserNameAndPasswordAsync(model);

            var superAdminRole =
                await _userService.GetRolesByName("SuperAdmin");

            if (applicationUser == null)
            {
                return BadRequest(new ErrorResponse(ApiErrors.BadRequest,
                    "Invalid UserName/Password.",
                    StatusCodes.Status400BadRequest));
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,
                    applicationUser.Id.ToString()),
                new(ClaimTypes.Name, applicationUser.UserName),
                new(ClaimTypes.Role, "Admin")
            };


            if (superAdminRole != null)
            {
                var userRole = await _userService.GetUserRoles(applicationUser.Id, superAdminRole.Id);
                if (userRole != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, "SuperAdmin"));
                }
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(issuer: issuer, audience: issuer, claims: claims,
                expires: DateTime.Now.AddDays(3), signingCredentials: credentials);
            String accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            LoginResponse loginResponse = new LoginResponse
            {
                UserId = applicationUser.Id,
                TokenType = "Bearer",
                AccessToken = accessToken,
                AccessTokenExpiresAtUtc = DateTime.Now.AddDays(3)
            };

            return Ok(loginResponse);
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