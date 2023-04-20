using Casheesh.Models;
using Casheesh.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Set Culture Environment Variable if empty
string culture = Environment.GetEnvironmentVariable("CULTURE") ?? Environment.GetEnvironmentVariable("Culture") ?? Environment.GetEnvironmentVariable("culture") ?? "en-US";
Environment.SetEnvironmentVariable("CULTURE", culture);

builder.Services.AddHostedService<CasheeshService>();
builder.Services.AddDbContext<CasheeshContext>();
builder.Services.AddLocalization();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateAsyncScope())
{
    CasheeshContext casheeshContext = scope.ServiceProvider.GetRequiredService<CasheeshContext>();
    casheeshContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(Environment.GetEnvironmentVariable("CULTURE") ?? "en-CA");

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

app.Run();