using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DatabaseContextConnection") ?? throw new InvalidOperationException("Connection string 'DatabaseContextConnection' not found.");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer("Server=LAPTOP-4OJRF6T6\\SQLEXPRESS;Database=PetReunionHub;Trusted_Connection=True"));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<DatabaseContext>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//{
//    // Configure identity options here
//})
//.AddEntityFrameworkStores<DatabaseContext>();

//// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

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

app.MapRazorPages();

app.Run();

