using dkp_system_back_front.Server;
using dkp_system_back_front.Server.Infrastructure.Data;
using WebUtilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddServices()
        //.AddSwagger()
        .ConfigureControllers()
        .AddApplicationDbContext<ApplicationDbContext>(builder.Configuration)
        //.AddRedis(builder.Configuration)
        //.AddRabbit(builder.Configuration)
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations<ApplicationDbContext>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
