using Microsoft.EntityFrameworkCore;
using UCE_TEST;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionStr = System.Environment.GetEnvironmentVariable("UCE_TEST_DB_CONNECTION") ?? "";

if (String.IsNullOrEmpty(connectionStr))
    throw new ApplicationException("UNABLE TO GET THE CONNECTION STRING, THEREFORE COULDN'T CONNECT TO DATABASE");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionStr));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

