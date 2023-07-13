using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sportal.Domain.Entities.DepartmentAggregate;
using Sportal.Domain.Entities.UsersAggregate;

namespace Sportal.Infrastructure.Persistence.RelationalDB;

public static class DatabaseSeeder
{
    public static async Task SeedIdentityRoles(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        SportalDbContext dbContext = serviceProvider.GetRequiredService<SportalDbContext>();
        string[] roleNames = {"SuperAdmin", "Admin"};

        foreach (string roleName in roleNames)
        {
            ApplicationRole? roleExist =
                await dbContext.Set<ApplicationRole>().FirstOrDefaultAsync(r => r.Name == roleName);
            if (roleExist != null)
            {
                continue;
            }

            // Create the roles and seed them to the database if they don't exist
            ApplicationRole applicationRole = new ApplicationRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            await dbContext.Set<ApplicationRole>().AddAsync(applicationRole);
        }

        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedInitialUsers(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        SportalDbContext dbContext = serviceProvider.GetRequiredService<SportalDbContext>();

        List<ApplicationUser> users = new List<ApplicationUser>
        {
            //System User used for seeds
            new()
            {
                UserName = "system",
                CNIC = "10202-8364197-7",
                Email = "system@sportal.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                PasswordHash = "system"
            }
        };
        foreach (ApplicationUser user in from user in users
                 let isExistent =
                     dbContext.Set<ApplicationUser>().Any(p => p.Email == user.Email && p.UserName == user.UserName)
                 where isExistent == false
                 select new ApplicationUser
                 {
                     Email = user.Email,
                     EmailConfirmed = user.EmailConfirmed,
                     PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                     UserName = user.UserName,
                     CNIC = user.CNIC,
                     PasswordHash = user.PasswordHash
                 })
        {
            await dbContext.Set<ApplicationUser>().AddAsync(user);
        }

        await dbContext.SaveChangesAsync();
    }
    
    public static async Task SeedDepartments(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        SportalDbContext dbContext = serviceProvider.GetRequiredService<SportalDbContext>();

        List<Department> departments = new List<Department>
        {
            //System User used for seeds
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Federal Crown corporations"
            },
            //System User used for seeds
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Intelligence agencies"
            },
            //System User used for seeds
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Agriculture and Agri-Food"
            }
        };
        foreach (Department department in from department in departments
                 let isExistent =
                     dbContext.Set<Department>().Any(d => d.Name == department.Name)
                 where isExistent == false
                 select new Department
                 {
                    Id = department.Id,
                    Name = department.Name
                 })
        {
            await dbContext.Set<Department>().AddAsync(department);
        }

        await dbContext.SaveChangesAsync();
    }
    
    public static async Task SeedSuperAdmins(IServiceProvider serviceProvider)
    {
        await Console.Out.WriteLineAsync("Seeding SuperAdmin Roles.");
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        string[] userNames =
        {
            "system"
        };
        SportalDbContext dbContext = serviceProvider.GetRequiredService<SportalDbContext>();
        ApplicationRole? superAdminRole =
            await dbContext.Set<ApplicationRole>().FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
        if (superAdminRole != null)
        {
            foreach (string userName in userNames)
            {
                ApplicationUser? userInfo =
                    await dbContext.Set<ApplicationUser>().FirstOrDefaultAsync(u => u.UserName == userName);

                if (userInfo == null)
                {
                    continue;
                }

                IdentityUserRole<Guid>? userRole = await dbContext.Set<IdentityUserRole<Guid>>()
                    .FirstOrDefaultAsync(ur => ur.UserId == userInfo.Id && ur.RoleId == superAdminRole.Id);
                //Only assign if user doesn't have the role.
                if (userRole == null)
                {
                    await dbContext.Set<IdentityUserRole<Guid>>().AddAsync(new IdentityUserRole<Guid>
                    {
                        UserId = userInfo.Id,
                        RoleId = superAdminRole.Id
                    });
                }
            }

            await dbContext.SaveChangesAsync();
        }

        await Console.Out.WriteLineAsync("Seeding SuperAdmin Roles Complete.");
    }
}