using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Rockaway.WebApp.Data;
using Rockaway.WebApp.Hosting;
using Rockaway.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(options => options.Conventions.AuthorizeAreaFolder("admin", "/"));
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IStatusReporter>(new StatusReporter());

var logger = CreateAdHocLogger<Program>();

logger.LogInformation("Rockaway running in {environment} environment", builder.Environment.EnvironmentName);
// A bug in .NET 8 means you can't call extension methods from Program.Main, otherwise
// the aspnet-codegenerator tools fail with "Could not get the reflection type for DbContext"
// ReSharper disable once InvokeAsExtensionMethod
if (HostEnvironmentExtensions.UseSqlite(builder.Environment)) {
	logger.LogInformation("Using Sqlite database");
	var sqliteConnection = new SqliteConnection("Data Source=:memory:");
	sqliteConnection.Open();
	builder.Services.AddDbContext<RockawayDbContext>(options => options.UseSqlite(sqliteConnection));
} else {
	logger.LogInformation("Using SQL Server database");
	var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
	builder.Services.AddDbContext<RockawayDbContext>(options => options.UseSqlServer(connectionString));
}

builder.Services.AddDefaultIdentity<IdentityUser>()
	.AddEntityFrameworkStores<RockawayDbContext>();

#if DEBUG && !NCRUNCH
builder.Services.AddSassCompiler();
#endif

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
	using var db = scope.ServiceProvider.GetService<RockawayDbContext>()!;
	if (app.Environment.UseSqlite()) {
		db.Database.EnsureCreated();
	} else if (Boolean.TryParse(app.Configuration["apply-migrations"], out var applyMigrations) && applyMigrations) {
		logger.LogInformation("apply-migrations=true was specified. Applying EF migrations and then exiting.");
		db.Database.Migrate();
		logger.LogInformation("EF database migrations applied successfully.");
		Environment.Exit(0);
	}
}


//if (HostEnvironmentExtensions.UseSqlite(app.Environment)) {
//	using var scope = app.Services.CreateScope();
//	using var db = scope.ServiceProvider.GetService<RockawayDbContext>()!;
//	db.Database.EnsureCreated();
//}

if (app.Environment.IsProduction()) {
	app.UseExceptionHandler("/Error");
	app.UseHsts();
} else {
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.MapGet("/status", (IStatusReporter reporter) => reporter.GetStatus());
app.MapAreaControllerRoute(
	name: "admin",
	areaName: "Admin",
	pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


app.Run();

ILogger<T> CreateAdHocLogger<T>() => LoggerFactory.Create(lb => lb.AddConsole()).CreateLogger<T>();