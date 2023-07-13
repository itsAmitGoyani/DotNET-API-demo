using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sportal.Application.Utils;

public class ProblemDetailsCleanupFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
    {
        if (schemaFilterContext.Type != typeof(ProblemDetails))
        {
            return;
        }

        if (schema.Properties.ContainsKey("instance"))
        {
            schema.Properties.Remove("instance");
        }

        if (schema.Properties.ContainsKey("extensions"))
        {
            schema.Properties.Remove("extensions");
        }

        schema.AdditionalPropertiesAllowed = false;
        schema.AdditionalProperties = null;

        if (schemaFilterContext.SchemaRepository.Schemas.ContainsKey("Object"))
        {
            schemaFilterContext.SchemaRepository.Schemas.Remove("Object");
        }
    }
}