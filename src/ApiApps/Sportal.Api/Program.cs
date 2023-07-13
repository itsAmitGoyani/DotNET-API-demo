using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyDataMyConsent.Application.Utils;
using Serilog;
using Sportal.Api.Extensions;
using Sportal.Infrastructure.Persistence.RelationalDB.Extensions;
using Sportal.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddServicesOfAllTypes();
    builder.Services.AddResponseCompression();

    builder.Services.AddRouting(options =>
    {
        options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
        options.LowercaseUrls = true;
    });

    builder.Services.AddSportalDbContext();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                ValidAudience = builder.Configuration.GetSection("Jwt:Issuer").Value,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
            };
        });

    builder.Services.AddCors(c =>
    {
        c.AddPolicy("AllowOrigin", options =>
        {
            options.AllowAnyOrigin();
            options.AllowAnyHeader();
            options.AllowAnyMethod();
        });
    });

    builder.Services.AddHttpContextAccessor();
    
    builder.Services.AddAuthorization(
        // options =>
        // {
        //     options.AddPolicy(
        //         "super_admin_access",
        //         policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin"));
        //     options.AddPolicy(
        //         "admin_access",
        //         policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
        // }
        );
    
    builder.Services.AddProblemDetails();

    builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase,
            false));
    }).ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            BadRequestObjectResult result = new BadRequestObjectResult(new ErrorResponse(ApiErrors.BadRequest,
                "Bad Request", StatusCodes.Status400BadRequest, context.ModelState));
            result.ContentTypes.Add(MediaTypeNames.Application.Json);
            result.ContentTypes.Add(MediaTypeNames.Application.Xml);
            return result;
        };
    });
    
    
    builder.Services.AddSwaggerSettings();
    
    builder.Services.AddMvcCore()
        .AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }).AddApiExplorer();
    
    builder.Services.AddMvc(options =>
    {
        // options.AddAudit();
        // options.Filters.Add(new AuditApiAttribute());
        options.EnableEndpointRouting = false;
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    app.UseStaticFiles();

    // app.UseSerilogRequestLogging();

    app.UseRouting();
    app.UseAuthentication();
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseAuthorization();    

    app.MapControllers();

    app.UseSwaggerUI(
    c =>
    {
        c.DocExpansion(DocExpansion.None);
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sportal API v1");
    });

    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
        {
            if (!httpRequest.Headers.ContainsKey("X-Forwarded-Host"))
            {
                return;
            }

            StringValues protocol = httpRequest.Headers["X-Forwarded-Proto"];
            if (!httpRequest.Headers.ContainsKey("X-Forwarded-Proto"))
            {
                protocol = "https";
            }

            string serverUrl =
                $"{protocol}://{httpRequest.Headers["X-Forwarded-Host"]}/{httpRequest.Headers["X-Forwarded-Prefix"]}";

            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
        });
    });
    
    app.UseMvc();
}

app.Run();
