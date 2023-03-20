using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Serilog;
using TestTaskApi.Extensions;
using TestTaskApi.Models.DbContext;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .WriteTo.Console().CreateLogger();
Log.Logger = logger;
Log.Information("Application is starting up...");

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.Enrich.WithProperty("ServiceName", "test-api");
    lc.WriteTo.Console(
            outputTemplate:
            "[{ServiceName} {Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .ReadFrom.Configuration(ctx.Configuration);
});

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Migrate latest database changes during startup
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    var migrateDbPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

    migrateDbPolicy.Execute(() =>
    {
        // Here is where the migration is proceeded
        dbContext.Database.Migrate();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Log.Information("Application is shutting down...");
Log.CloseAndFlush();