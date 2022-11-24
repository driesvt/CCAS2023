using CCAS.BlazorOldServer.Data;
using CCAS.BlazorOldServer.Services;
using CCAS.Application;
using CCAS.Application.Common.Interfaces;
using CCAS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using CCAS.BlazorOldServer;
using MudBlazor.Services;
using CCAS.Application.Common.Persistence;
using CCAS.BlazorOldServer.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration, ServiceLifetime.Transient);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddMudServices();

builder.Services.AddScoped<IAppDialogService, AppDialogService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();

//builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = true; }); 
// JavaScript isolation is marked as obsolete and disabled by default. You don’t have to make below additional changes. See https://blazor.syncfusion.com/documentation/common/adding-script-references

builder.Services.AddSingleton<WeatherForecastService>();

// Register all Syncfusion Data Adapters
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
        .AddClasses(classes => classes.AssignableTo<DataAdaptor>())
        .AsSelf()
        .WithTransientLifetime()
);

// Add ADFS Authentication
//builder.Services.AddOIDCAuthentication(builder.Configuration);
builder.Services.AddFakeAuthentication(builder.Configuration);

// Add Authorisation Policies
builder.Services.AddApplicationAuthorization();

// Add Logging
builder.AddCustomLogging(builder.Configuration, stack: "CCAS");


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsSqlServer())
        {
            context.Database.Migrate();
        }

        await ApplicationDbContextSeed.SeedSampleDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Register OIDC Authenciation
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
