using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Feddit_Domain.Connections;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddRazorPages();

var _connectionString = Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddRazorPages().Services.AddSingleton<Connection>(new Connection(_connectionString))
.AddSession(option => { option.IdleTimeout = TimeSpan.FromMinutes(30); })
.AddMemoryCache()
.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();