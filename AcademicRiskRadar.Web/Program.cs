using System;
using AcademicRiskRadar.Web.Components;
using AcademicRiskRadar.Web.Middleware;
using AcademicRiskRadar.Web.Models;
using AcademicRiskRadar.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC Controllers & Views (Unit III)
builder.Services.AddControllersWithViews();

// 2. Add Blazor Interactive Server Components
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// 3. Add Session & State Management (Unit III)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".AcademicRiskRadar.Session";
});

// 4. Add Authentication & Authorization (Unit IV)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AcademicRiskRadar.Auth";
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// 5. Register Domain Repositories & Business Services (Unit II & IV Dependency Injection)
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<StandardRiskCalculator>();
builder.Services.AddScoped<WeightedTrendRiskCalculator>();
builder.Services.AddScoped<IRiskCalculator, StandardRiskCalculator>();
builder.Services.AddScoped<CSharpFundamentalsLabService>();

// Blazor shared demo services
builder.Services.AddScoped<AcademicDataService>();
builder.Services.AddScoped<AcademicWorkflowService>();

// 6. Register Dependency Injection Lifetime Demonstrations (Unit IV: Transient, Scoped, Singleton)
builder.Services.AddTransient<IOperationTransient, OperationTransient>();
builder.Services.AddScoped<IOperationScoped, OperationScoped>();
builder.Services.AddSingleton<IOperationSingleton, OperationSingleton>();
builder.Services.AddTransient<IOperationService, OperationService>();

var app = builder.Build();

// Configure the HTTP request pipeline (Unit IV Middleware Pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Unit IV: Custom Middleware 1 - Request Execution Timing
app.UseMiddleware<RequestExecutionTimingMiddleware>();

// Built-in Static Files & Static Assets
app.UseStaticFiles();
app.MapStaticAssets();

// Routing Middleware
app.UseRouting();

// Unit III: Session Middleware
app.UseSession();

// Unit IV: Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// Unit IV: Custom Middleware 2 - Security Audit Logging
app.UseMiddleware<AcademicSecurityAuditMiddleware>();

app.UseAntiforgery();

// Map MVC Controllers & Routes
// Standard MVC Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Convention Routing Example
app.MapControllerRoute(
    name: "api_convention",
    pattern: "api/convention/{controller}/{action}/{id?}"
);

// Unit VI: Attribute-routed Web API endpoints
app.MapControllers();

// Blazor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
