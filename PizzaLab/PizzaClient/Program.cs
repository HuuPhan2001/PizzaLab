using BusinessObject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PizzaClient.MiddeWare;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtCookieMiddleware";
    options.DefaultChallengeScheme = "JwtCookieMiddleware";
})
.AddScheme<AuthenticationSchemeOptions, JwtCookieAuthenticationHandler>("JwtCookieMiddleware", options => { });
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//	// This lambda determines whether user consent for non-essential cookies is needed for a given request.
//	options.CheckConsentNeeded = context => true;
//	options.MinimumSameSitePolicy = SameSiteMode.None;
//});
builder.Services.AddCors();
builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);
builder.Services.AddSession(options =>
{
	// Set session timeout
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=OpenLogin}");
app.UseStaticFiles();
app.UseCookiePolicy();

app.Run();
