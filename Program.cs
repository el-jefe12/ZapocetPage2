using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TexasGuyContractIdentity.Data;
using TexasGuyContractIdentity.Models.Services;
using TexasGuyContractIdentity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Retrieve the connection string from the configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add database developer page exception filter for detailed errors during development
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with default settings and require confirmed accounts for sign-in
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add custom service for handling history entries
builder.Services.AddScoped<HistoryEntryService>();

// Add Hangfire with SQL Server storage
builder.Services.AddHangfire(configuration => configuration
    .UseSqlServerStorage(connectionString));

// Add Razor Pages and controllers
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddTransient<TokenService>();
// Register EmailSender service
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Detailed error page for development
    app.UseMigrationsEndPoint();     // Endpoints for applying migrations
}
else
{
    app.UseExceptionHandler("/Error"); // Custom error page in production
    app.UseHsts();                     // HTTP Strict Transport Security
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles();      // Serve static files

app.UseRouting();          // Enable routing

app.UseAuthentication();   // Enable authentication
app.UseAuthorization();    // Enable authorization

app.UseHangfireServer();   // Use Hangfire server
app.UseHangfireDashboard(); // Use Hangfire dashboard (optional, for monitoring)

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();  // Map Razor Pages
    endpoints.MapControllers(); // Map API controllers
});

// Register a Hangfire job to run after the application starts
app.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var scope = scopeFactory.CreateScope())
    {
        var historyService = scope.ServiceProvider.GetRequiredService<HistoryEntryService>();
        RecurringJob.AddOrUpdate("update-all-stations", () => historyService.UpdateAllStationsAsync(), Cron.Minutely);
    }
});

app.Run();
