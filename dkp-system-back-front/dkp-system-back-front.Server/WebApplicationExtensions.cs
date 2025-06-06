using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebUtilities;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigrations<TContext>(this WebApplication app) where TContext : DbContext
    {
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider serviceProvider = scope.ServiceProvider;
        TContext context = serviceProvider.GetRequiredService<TContext>();
        context.Database.Migrate();

        return app;
    }
}
