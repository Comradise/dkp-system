using MassTransit;
using WebUtilities;
using Microsoft.AspNetCore.Hosting;
using Sentry.Extensibility;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using dkp_system_back_front.Server.Core.Services.Interfaces;
using dkp_system_back_front.Server.Core.Services.Implementations;
using Microsoft.Extensions.Configuration;
using dkp_system_back_front.Server.Infrastructure.Data;
using dkp_system_back_front.Server.Core.Models.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace dkp_system_back_front.Server;
public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IGuildService, GuildService>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentification(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddAuthentication()
            .AddCookie(IdentityConstants.ApplicationScheme)
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();
        /*services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    )
                };
            });*/

        return services;
    }

    public static IServiceCollection AddRabbit(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMassTransit(x =>
        {
            // x.AddConsumer<SampleMessageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("Rabbit"));

                // cfg.ReceiveEndpoint("sample-queue", e => { e.ConfigureConsumer<SampleMessageConsumer>(context); });
            });
        });
        return services;
    }

    public static IWebHostBuilder ConfigureSentry(
        this ConfigureWebHostBuilder webHost,
        ConfigurationManager configuration)
    {
        webHost.UseSentry(options =>
        {
            options.Dsn = configuration.GetConnectionString("Sentry") ?? string.Empty;
            /*options.AddExceptionFilterForType<BadRequestProblemException>();
            options.AddExceptionFilterForType<FailedDependencyProblemException>();
            options.AddExceptionFilterForType<ForbiddenProblemException>();
            options.AddExceptionFilterForType<NotFoundProblemException>();
            options.AddExceptionFilterForType<UnauthorizedProblemException>();
            options.AddExceptionFilterForType<HttpProblemException>();*/
            options.Debug = true;
            options.SendDefaultPii = true;
            options.MaxRequestBodySize = RequestSize.Always;
            options.TracesSampleRate = 1.0;
            options.AttachStacktrace = true;
            options.IncludeActivityData = true;
        });

        return webHost;
    }

    public static IServiceCollection AddSwagger(
        this IServiceCollection services,
        Action<SwaggerGenOptions>? setupAction = null)
    {
        services.AddSwaggerGen(opt =>
        {
            string xmlFile = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opt.IncludeXmlComments(xmlPath);
            /*opt.OperationFilter<SortSchemaFilter>();
            opt.OperationFilter<RemoveOtherMediaTypesFilter>();
            opt.OperationFilter<ValidationProblemDetailsFilter>();
            opt.DocumentFilter<AddValidationProblemDetailsDocumentFilter>();
            opt.SchemaFilter<FluentValidationRules>();*/
            setupAction?.Invoke(opt);
        });
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers().ConfigureApiBehaviorOptions(delegate (ApiBehaviorOptions options)
        {
            options.InvalidModelStateResponseFactory = delegate (ActionContext context)
            {
                ModelStateDictionary modelState = context.ModelState;
                ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails
                {
                    Title = "One or more validation errors occurred",
                    Detail = "Validation failed for one or more properties. See the 'Errors' property for details.",
                    Status = 400
                };
                foreach (string key in modelState.Keys)
                {
                    string[] array = modelState[key].Errors.Select((ModelError e) => e.ErrorMessage).ToArray();
                    if (array.Length != 0)
                    {
                        validationProblemDetails.Errors[key] = array;
                    }
                }

                return new ObjectResult(validationProblemDetails)
                {
                    ContentTypes = { "application/json" }
                };
            };
        }).AddJsonOptions(delegate (JsonOptions jsonOptions)
        {
            jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, ConfigurationManager configuration)
    {
        ConfigurationManager configuration2 = configuration;
        services.AddStackExchangeRedisCache(delegate (RedisCacheOptions redisOptions)
        {
            redisOptions.Configuration = configuration2.GetConnectionString("Redis");
        });
        return services;
    }

    public static IServiceCollection AddApplicationDbContext<TContext>(this IServiceCollection services, ConfigurationManager configuration) where TContext : IdentityDbContext<ApplicationUser>
    {
        services.AddDbContext<TContext>(delegate (DbContextOptionsBuilder options)
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"), delegate (NpgsqlDbContextOptionsBuilder opt)
            {
                opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });
        return services;
    }
}