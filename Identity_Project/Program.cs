using Identity_Project.Data;
using Identity_Project.Models;
using Identity_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MADbContext>(opt => 
        opt.UseSqlServer(builder.Configuration.GetConnectionString("identityConn")));







// How to setup an identity service management?

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<MADbContext>()
    .AddDefaultTokenProviders();




AutoMapperConfig.Configuration();

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


app.UseAuthentication();


app.UseAuthorization();


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapAreaControllerRoute(

//        name: "areas",
//        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//});


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



    
    



app.Run();
