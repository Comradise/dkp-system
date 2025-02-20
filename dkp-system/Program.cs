using dkp_system;
using dkp_system.Infrastructure.Data;
using WebUtilities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    builder.WebHost
        .ConfigureSentry(builder.Configuration);

    builder.Services
        .AddServices()
        .AddSwagger()
        .ConfigureControllers()
        .AddApplicationDbContext<ApplicationDbContext>(builder.Configuration)
        .AddRedis(builder.Configuration)
        .AddRabbit(builder.Configuration)
        ;
}
WebApplication app = builder.Build();
{
    app.ApplyMigrations<ApplicationDbContext>();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapControllers();
    app.Run();
}
