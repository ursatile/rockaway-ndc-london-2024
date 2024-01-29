using Rockaway.WebApp.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IStatusReporter>(new StatusReporter());
var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.MapGet("/status", (IStatusReporter reporter) => reporter.GetStatus());

app.Run();

