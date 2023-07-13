using System.Reflection;
using Microsoft.OpenApi.Models;
using Sportal.Application.Utils;

namespace Sportal.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSwaggerSettings(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSwaggerGen(c =>
        {
            c.UseOneOfForPolymorphism();
            c.SelectSubTypesUsing(baseType =>
            {
                return typeof(Program).Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType));
            });
            c.DescribeAllParametersInCamelCase();
            c.SchemaFilter<ProblemDetailsCleanupFilter>();
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Sportal Internal API",
                });
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
            c.EnableAnnotations();
            
            // Set the comments path for the Swagger JSON and UI.
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }
}