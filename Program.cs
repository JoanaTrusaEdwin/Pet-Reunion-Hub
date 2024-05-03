using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Pet_Reunion_Hub.Interfaces;
using Pet_Reunion_Hub.Controllers;




var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DatabaseContextConnection") ?? throw new InvalidOperationException("Connection string 'DatabaseContextConnection' not found.");
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer("Server=LAPTOP-4OJRF6T6\\SQLEXPRESS;Database=PetReunionHub;Trusted_Connection=True"));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<DatabaseContext>();

builder.Services.AddScoped<NotificationController>();

//builder.Services.ConfigureApplicationCookie(options =>
//{

//    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie expires when browser session ends
//    options.SlidingExpiration = false;
//});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLoggedIn", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/", "RequireLoggedIn");
    options.Conventions.AllowAnonymousToPage("/Index");

    options.Conventions.ConfigureFilter(new AuthorizeFilter("RequireLoggedIn"));
});


builder.Logging.AddConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
});

var app = builder.Build();


// Countdown Timer for Session Expiry Warning
//app.Use(async (context, next) =>
//{
//    var cookieExpireTime = TimeSpan.FromMinutes(30); // Cookie expiry time
//    var warningTime = TimeSpan.FromMinutes(3); // Warning time before expiry

//    var timeUntilExpiration = context.Request.Cookies[".AspNetCore.Identity.Application"] != null
//        ? cookieExpireTime - (DateTimeOffset.UtcNow - DateTimeOffset.Parse(context.Request.Cookies[".AspNetCore.Identity.Application"]))
//        : TimeSpan.Zero;

//    if (timeUntilExpiration <= warningTime && timeUntilExpiration > TimeSpan.Zero)
//    {
//        // Display a warning message to the user
//        // This could be a pop-up notification or a countdown timer in the UI
//        Console.WriteLine("Your session will expire in three minutes. Please save your work or extend your session.");
//    }

//    await next();
//});


//Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    //The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();

app.Run();

