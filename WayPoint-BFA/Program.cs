using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using WayPoint.Model;
using WayPoint_Infrastructure.Data;
using WayPoint_Infrastructure.Interfaces;
using WayPoint_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<WayPointDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging());

builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<WayPointDbContext>());

builder.Services.AddScoped<ISqlEngine, SqlEngine>();
builder.Services.AddScoped<IEfReadEngine<WayPointDbContext>, EfReadEngine<WayPointDbContext>>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ISystemWorkOrderRepository, SystemWorkOrderRepository>();
builder.Services.AddScoped<IWorkOrder, WorkorderRepository>();
builder.Services.AddScoped<ISystemDiscountRepository, SystemDiscountRepository>();
builder.Services.AddScoped<ILookUpRepository, LookUpRepository>();
builder.Services.AddScoped<IClientAgreementRepository, ClientAgreementRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
});

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());
builder.Services.AddHealthChecksUI(setupSettings =>
{
    setupSettings.AddHealthCheckEndpoint("WayPoint API - self", "/health");
})
.AddInMemoryStorage();

// ---- CORS ----
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("DefaultCorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.Run();