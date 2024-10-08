using B8Auth2.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;


var configBuilder = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json", true, true)
                         .AddJsonFile($"appsettings.Development.json", true, true)
                         .AddEnvironmentVariables()
                         .AddUserSecrets<Program>();

IConfiguration configuration = configBuilder.Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;    
})
.AddCookie()
.AddMicrosoftAccount(microsoftOptions =>
{    
    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"]!;
    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"]!;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
