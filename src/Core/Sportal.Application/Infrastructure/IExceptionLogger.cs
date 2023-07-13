using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Sportal.Application;

[ScopedService]
public interface IExceptionLogger
{
    Task LogAsync(Exception exception);

    Task LogAsync(Exception exception, object paramters);
}