
using Blomsterbinderiet.EFDbContext;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BlomstDbContext>();
builder.Services.AddTransient<DbGenericService<User>, DbGenericService<User>>();
builder.Services.AddTransient<DbGenericService<Product>, DbGenericService<Product>>();
builder.Services.AddTransient<DbGenericService<Keyword>, DbGenericService<Keyword>>();
builder.Services.AddTransient<DbGenericService<Order>, DbGenericService<Order>>();
builder.Services.AddSingleton<UserService>();



builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request. options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cookieOptions => {
    cookieOptions.LoginPath = "/Customer/Login";

});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run(); 