using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity;
using Sportal.Domain.Entities;
using Sportal.Domain.Entities.UsersAggregate;
using Sportal.Infrastructure.Persistence.RelationalDB.EntityConfigurations.UserAggregate;
using Sportal.Infrastructure.Persistence.RelationalDB.Extensions;

namespace Sportal.Infrastructure.Persistence.RelationalDB;

public class SportalDbContext: AuditIdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
   public SportalDbContext(DbContextOptions<SportalDbContext> options) : base(options)
       {
       }
   
       protected override void OnModelCreating(ModelBuilder builder)
       {
           if (builder == null)
           {
               throw new ArgumentNullException(nameof(builder));
           }
   
           base.OnModelCreating(builder);
   
           builder.Entity<ApplicationUser>().ToTable("Users");
           builder.Entity<ApplicationRole>().ToTable("Roles");
           builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
   
           builder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserConfiguration).Assembly);
   
           // This should be called after calling the derived entity configurations
           builder.ApplyBaseEntityConfiguration();
   
           // This should be called after calling the derived entity configurations
           builder.ApplySoftDeleteEntityConfiguration();
       }
   
       public override int SaveChanges()
       {
           UpdateSoftDeleteStatuses();
           return base.SaveChanges();
       }
   
       public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default(CancellationToken))
       {
           UpdateSoftDeleteStatuses();
           return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
       }
   
       private void UpdateSoftDeleteStatuses()
       {
           foreach (EntityEntry entry in ChangeTracker.Entries())
           {
               if (entry.Entity.GetType().IsSubclassOf(typeof(SoftDeletableEntity)))
               {
                   switch (entry.State)
                   {
                       case EntityState.Added:
                           entry.CurrentValues["DeletedAtUtc"] = null;
                           break;
                       case EntityState.Deleted:
                           entry.State = EntityState.Modified;
                           entry.CurrentValues["DeletedAtUtc"] = DateTime.UtcNow;
                           break;
                       case EntityState.Detached:
                           break;
                       case EntityState.Unchanged:
                           break;
                       case EntityState.Modified:
                           break;
                   }
               }
           }
       }
}