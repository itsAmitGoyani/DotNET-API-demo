using System.Diagnostics;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sportal.Infrastructure.Persistence.RelationalDB;
using Sportal.Infrastructure.Persistence.RelationalDB.Extensions;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Sportal.DB.Migrator;

public class Program
{
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();
        ServiceCollection services = new ServiceCollection();

        services.AddServicesOfAllTypes();
        services.AddSingleton(configuration);
        services.AddSportalDbContext();
        DiagnosticListener listener = new DiagnosticListener("Sportal.DB.Migrator");
        services.AddSingleton(listener);
        services.AddSingleton<DiagnosticSource>(listener);

        string databaseHostname = Environment.GetEnvironmentVariable("DATABASE_HOST")!;
        string databasePort = Environment.GetEnvironmentVariable("DATABASE_PORT")!;
        string databaseUsername = Environment.GetEnvironmentVariable("DATABASE_USERNAME")!;
        string databasePassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD")!;
        string databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME")!;
        string connectionString =
            $"Host={databaseHostname};Port={databasePort};Username={databaseUsername};Password={databasePassword};SSL Mode=Require;Trust Server Certificate=true;Database={databaseName};";

        services.AddIdentityServer()
            // this adds the config data from DB (clients, resources, CORS)
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly("Sportal.Infrastructure.Persistence");
                        npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory");
                    });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                {
                    builder.UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly("Sportal.Infrastructure.Persistence");
                        npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory");
                    });
                };
            });

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        Console.WriteLine("Running migrations...");
        SportalDbContext sportalDbContext =
            serviceProvider.GetRequiredService<SportalDbContext>();
        await sportalDbContext.Database.MigrateAsync();

        ConfigurationDbContext configurationDbContext =
            serviceProvider.GetRequiredService<ConfigurationDbContext>();
        await configurationDbContext.Database.MigrateAsync();

        PersistedGrantDbContext persistedGrantDbContext =
            serviceProvider.GetRequiredService<PersistedGrantDbContext>();

        await persistedGrantDbContext.Database.MigrateAsync();
        Console.WriteLine("Migrations complete!");

        //Run Seeding Operations
        Console.WriteLine("Seeding database...");
        //Initial Users
        await DatabaseSeeder.SeedInitialUsers(serviceProvider);
        //Identity Roles
        await DatabaseSeeder.SeedIdentityRoles(serviceProvider);
        //Seed User SuperAdmin Role
        await DatabaseSeeder.SeedSuperAdmins(serviceProvider);
        //Seed User Subscription Plan
        await DatabaseSeeder.SeedDepartments(serviceProvider);
        
        Console.WriteLine("Seeding database complete!");
    }
}
