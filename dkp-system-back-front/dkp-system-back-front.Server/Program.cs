using System.Security.Claims;
using dkp_system_back_front.Server;
using dkp_system_back_front.Server.Core.Models.Authorization;
using dkp_system_back_front.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebUtilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddServices(builder.Configuration)
        //.AddSwagger()
        .ConfigureControllers()
        .AddApplicationDbContext<ApplicationDbContext>(builder.Configuration)
        //.AddRedis(builder.Configuration)
        //.AddRabbit(builder.Configuration)
        .AddJwtAuthentification(builder.Configuration)
        ;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseDefaultFiles();
app.UseStaticFiles();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.ApplyMigrations<ApplicationDbContext>();

app.UseHttpsRedirection();

app.MapIdentityApi<ApplicationUser>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
