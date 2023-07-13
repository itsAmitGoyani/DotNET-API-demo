using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace MyDataMyConsent.Application.Utils;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        // Slugify value
        return value == null ? null : Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLowerInvariant();
    }
}
