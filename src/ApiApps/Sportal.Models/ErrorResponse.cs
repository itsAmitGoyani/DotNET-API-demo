using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sportal.Models;

public class ErrorResponse
{
    public const string InsufficientPermissionMessage =
        "You do not have required permissions to perform this operation";

    public ErrorResponse(ApiErrors type, string title, int status)
    {
        Type = type;
        Title = title;
        Status = status;
    }

    public ErrorResponse(ApiErrors type, string title, int status, ModelStateDictionary validationErrors)
    {
        Type = type;
        Title = title;
        Status = status;
        Errors = validationErrors.Select(item => item.Value.Errors).Where(item => item.Count > 0)
            .Select(err => err.FirstOrDefault().ErrorMessage);
    }

    public ErrorResponse(ApiErrors type, string title, int status, int code, string detail)
    {
        Type = type;
        Title = title;
        Status = status;
        Code = code;
        Detail = detail;
    }

    public ErrorResponse(ApiErrors type, string title, int status, int code, HttpContext httpContext)
    {
        Type = type;
        Title = title;
        Status = status;
        Code = code;
        TraceId = httpContext.TraceIdentifier;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public ApiErrors Type { get; }

    public string Title { get; }
    public int? Status { get; }
    public int? Code { get; }
    public string Detail { get; }
    public string TraceId { get; set; }
    public IEnumerable<string> Errors { get; set; }
}