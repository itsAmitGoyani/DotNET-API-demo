using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Errors.Model;
using Sportal.Domain.Entities.UsersAggregate;
using Sportal.Models;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Sportal.Application.Services;

[ScopedService]
public class UserService
{
    private readonly IRepository _repository;

    public UserService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationUser?> GetUserInfoByUserNameAndPasswordAsync(LoginRequestModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        ApplicationUser? user =
            await _repository.GetAsync<ApplicationUser>(e =>
                e.UserName == model.UserName && e.PasswordHash == model.Password);

        return user;
    }

    public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
    {
        return await _repository.GetAsync<ApplicationUser>(user => user.UserName == userName);
    }

    public async Task<ApplicationRole?> GetRolesByName(string name)
    {
        return await _repository.GetAsync<ApplicationRole>(user => user.Name == name);
    }

    public async Task<IdentityUserRole<Guid>?> GetUserRoles(Guid userId, Guid roleId)
    {
        return await _repository.GetAsync<IdentityUserRole<Guid>>(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
}