using Identity_Project.Data;
using Identity_Project.Helpers;
using Identity_Project.Models;
using Identity_Project.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MADbContext>(opt => 
        opt.UseSqlServer(builder.Configuration.GetConnectionString("identityConn")));


// How to setup an identity service management and other setting?

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<MADbContext>()
    .AddDefaultTokenProviders()

    // برای فارسی کردن خطاها 
    .AddErrorDescriber<CustomIdentityError>()

    // برای تایید اعتبار رمزهای عبور
    .AddPasswordValidator<PasswordValidator>()


    // اضافه کردن رول ها به کِلِیم های کاربر
    .AddRoles<Role>();

// اگر نیاز به سرویس های آیدنتیتی در سرویس های اضافه کردن کلیم های اختصاصی مثل دو سرویس پایین داریم
// باید دو خط زیر را بالاتر از آن سرویس ها قرار دهیم چون در کانسراکتور سرویس های کلیم به آن ها نیاز داریم
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddScoped<UserManager<User>>();


// Below line activate the IUserClaimsPrincipalFactory and its implementation
//خط زیر باید کامنت باشد تا بتوانیم رول هارا به کلیم های کاربر اضافه کنیم
///* this line should be commented */ builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, AddCustomClaim>();

// اما به جای سرویس بالا که کامنت شد
// از سرویس پایین برای اضافه کردن کلیم های مورد نیاز خودمون استفاده میکنیم
// دقت کنید کلیم های تولید شده توسط هم سرویس بالا که کامنت شد و هم سرویس پایین موقت بوده و روی کوکی مرورگر ذخیره میشوند
builder.Services.AddScoped<IClaimsTransformation, CustomClaimTransform>();





// for each handler you write, you need a an activator in program.cs

// to activate AuthorizationHandler and its implementation
builder.Services.AddSingleton<IAuthorizationHandler, UserAgeHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, UserBlogHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, UserBlogListDTOHandler>(); 

builder.Services.AddAuthorization(options =>
{
    // user should have "sa" role and also "sasaSignedIn"
    options.AddPolicy("saRolePolicy", policy =>
    {
        policy.RequireRole("sa");
        policy.RequireClaim("saSignedIn");
    });
    // user should have authenticated and has "VerifiedAge" claim with value 18 or higher 
    options.AddPolicy("VerifiedAgePolicy", policy =>
    {
       policy.RequireAuthenticatedUser();
       policy.RequireClaim("VerifiedAge", "true");
    });
    // user should has "VerifiedAge" claim with value 18 or higher 
    options.AddPolicy("VerifiedAgeByHandler", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new UserClaimsRequirements(18));
    });
    //***************************************************************
    // policy to check author and editor of a blog be the same person
    options.AddPolicy("UserBlogPolicyHandler", policy =>
    {
        policy.AddRequirements(new UserClaimsRequirements());
    });
    // User should have admin role to authorize
    options.AddPolicy("AdminVerifiedPolicy", policy =>
    {
        policy.RequireRole("Admin");
    });
});




builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "410039922230-1gpsgskirrgc7dih79oht422q0g3r9ib.apps.googleusercontent.com";
        options.ClientSecret = "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";

    });














//How to change Identity options?

builder.Services.Configure<IdentityOptions>(option =>
{
    //User options
    option.User.RequireUniqueEmail = true;
    //option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrwxyz";
    
    //Sign In options
    option.SignIn.RequireConfirmedPhoneNumber = false;
    option.SignIn.RequireConfirmedEmail = false;
    option.SignIn.RequireConfirmedAccount = false;

    //Stores options
    //option.Stores.MaxLengthForKeys = 50; // Max length for all keys in tables like UserId, RoleId etc.
    
    //Locked out options
    option.Lockout.AllowedForNewUsers = true;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
    option.Lockout.MaxFailedAccessAttempts = 5;
    
    // Tokens

    // ClaimsIdentity
});

// Cookies options
builder.Services.ConfigureApplicationCookie(option =>
{
    //option.AccessDeniedPath = "/Account/AccessDenied";
    option.LogoutPath = "/";
    //option.LoginPath = "/";
    option.ExpireTimeSpan = TimeSpan.FromDays(1);
    option.SlidingExpiration = true;
});





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



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





    
    



app.Run();
