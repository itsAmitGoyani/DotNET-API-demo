
using Audit.Core;
using Audit.Core.ConfigurationApi;
using Audit.EntityFramework;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sportal.Application.Services;
using Sportal.Domain.Entities.UsersAggregate;
using TanvirArjel.EFCore.GenericRepository;
using Configuration = Audit.Core.Configuration;

namespace Sportal.Infrastructure.Persistence.RelationalDB.Extensions;

public static class DbContextStartupExtensions
{
    public static void AddSportalDbContext(this IServiceCollection services)
    {
        string databaseHostname = Environment.GetEnvironmentVariable("DATABASE_HOST")!;
        string databasePort = Environment.GetEnvironmentVariable("DATABASE_PORT")!;
        string databaseUsername = Environment.GetEnvironmentVariable("DATABASE_USERNAME")!;
        string databasePassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD")!;
        string databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME")!;
        string databaseSslMode = Environment.GetEnvironmentVariable("DATABASE_SSL_MODE")!;

        if (databaseSslMode == string.Empty)
        {
            databaseSslMode = "Require";
        }

        string connectionString =
            $"Host={databaseHostname};Port={databasePort};Username={databaseUsername};Password={databasePassword};SSL Mode={databaseSslMode};Trust Server Certificate=true;Database={databaseName};";
        //configuration.GetConnectionString("Postgres");

        AddSportalDbContext(services, connectionString);
    }
    
    private static void AddSportalDbContext(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string is either null or empty.");
        }

        services.AddGenericRepository<SportalDbContext>();

        services.AddDbContextPool<SportalDbContext>((serviceProvider,
            options) =>
        {
            options.LogTo(Console.WriteLine, LogLevel.Warning);
            //options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsAssembly("Sportal.Infrastructure.Persistence");
                builder.MigrationsHistoryTable("__EFMigrationsHistory");
            });
            // if (!configuration.GetValue<bool>("DisableCachingInterceptor"))
            // {
            //     options.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
            // }
        });

        ConfigurationStoreOptions configurationStoreOptions = new ConfigurationStoreOptions();
        services.AddSingleton(configurationStoreOptions);

        OperationalStoreOptions operationalStoreOptions = new OperationalStoreOptions();
        services.AddSingleton(operationalStoreOptions);

        services.AddDbContext<ConfigurationDbContext>((serviceProvider,
            options) =>
        {
            options.LogTo(Console.WriteLine, LogLevel.Warning);
            //options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsAssembly("Sportal.Infrastructure.Persistence");
                builder.MigrationsHistoryTable("__EFMigrationsHistory");
            });
            // if (!configuration.GetValue<bool>("DisableCachingInterceptor"))
            // {
            //     options.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
            // }
        });

        services.AddDbContext<PersistedGrantDbContext>((serviceProvider,
            options) =>
        {
            options.LogTo(Console.WriteLine, LogLevel.Warning);
            //options.EnableSensitiveDataLogging();
            options.UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsAssembly("Sportal.Infrastructure.Persistence");
                builder.MigrationsHistoryTable("__EFMigrationsHistory");
            });
            // if (!configuration.GetValue<bool>("DisableCachingInterceptor"))
            // {
            //     options.AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
            // }
        });

        // Setup AuditLogging
        IConfigurator auditConfigSetup = Configuration.Setup();
        //Postgres for Mvc & Api Audit logs.
        auditConfigSetup.UsePostgreSql(config => config
            .ConnectionString(connectionString)
            .TableName("AuditLogs")
            .IdColumnName("Id")
            .DataColumn("AuditData")
            .LastUpdatedColumnName("AuditTimeUtc")
            .CustomColumn("Id", _ => Guid.NewGuid())
            .CustomColumn("AuditTimeUtc", ev => ev.StartDate.ToUniversalTime())
            .CustomColumn("EventType", ev => ev.EventType)
            .CustomColumn("AuditAction", ev => ev switch
            {
                AuditEventEntityFramework => ev.GetEntityFrameworkEvent().Entries.FirstOrDefault()?.Action
                    .ToString(),
                // AuditEventWebApi => ev.GetWebApiAuditAction().ControllerName + ":"+ev.GetWebApiAuditAction().ActionName.ToString(),
                // AuditEventMvcAction => ev.GetMvcAuditAction().ActionName + ":"+ev.GetWebApiAuditAction().ActionName.ToString(),
                _ => ""
            })
            .CustomColumn("EntityType", ev => ev switch
            {
                AuditEventEntityFramework => ev.GetEntityFrameworkEvent().Entries.FirstOrDefault()?.EntityType !=
                                             null
                    ? ev.GetEntityFrameworkEvent().Entries.FirstOrDefault()?.EntityType.Name.ToString()
                    : "",
                _ => ""
            })
            .CustomColumn("UserName", ev => ev.Environment.UserName)
            .CustomColumn("TablePk",
                ev => ev is AuditEventEntityFramework
                    ? ev.GetEntityFrameworkEvent().Entries.FirstOrDefault()?.PrimaryKey.FirstOrDefault().Value
                        .ToString() ?? ""
                    : "")
            //TODO::Add below
            // .CustomColumn("UserId",
            //     ev => ev.Environment.CustomFields.ContainsKey("UserId") &&
            //           ev.Environment.CustomFields["UserId"] != null
            //         ? Guid.Parse(ev.Environment.CustomFields["UserId"].ToString()!)
            //         : SystemUserDetails.Id)
        );
        // //EntityFramework audit event logs.
        //  auditConfigSetup.UseEntityFramework(_ => _
        //     .AuditTypeMapper(_ => typeof(AuditLog))
        //      .AuditEntityAction<AuditLog>((ev, entry, entity) =>
        //      {
        //          entity.AuditData = entry.ToJson();
        //          entity.AuditAction = entry.Action;
        //          entity.EntityType = entry.EntityType.Name;
        //          entity.AuditTimeUtc = DateTime.UtcNow;
        //          entity.EventType = "EntityFramework";
        //          entity.AuditUserId = ev.Environment.CustomFields.ContainsKey("UserId")
        //              ? ev.Environment.CustomFields["UserId"]?.ToString()
        //              : null;
        //          entity.AuditUserName = ev.Environment.UserName;
        //          entity.TablePk = entry.PrimaryKey.FirstOrDefault().Value?.ToString();
        //      }).IgnoreMatchedProperties());

        Audit.EntityFramework.Configuration.Setup()
            .ForContext<SportalDbContext>(config => config
                .AuditEventType("EntityFramework:{context}")
                .ForEntity<ApplicationUser>(_ => _
                    .Override(user => user.PasswordHash, null)));

        Audit.EntityFramework.Configuration.Setup()
            .ForContext<ConfigurationDbContext>(config => config
                .AuditEventType("EntityFramework:{context}"));

        Audit.EntityFramework.Configuration.Setup()
            .ForContext<PersistedGrantDbContext>(config => config
                .AuditEventType("EntityFramework:{context}"));
    }
}