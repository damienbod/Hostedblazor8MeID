using BlazorWebMeID.Client;
using BlazorWebMeID.Client.Pages;
using BlazorWebMeID.Client.Services;
using BlazorWebMeID.Identity;
using BlazorWebMeID.Services;
using HostedBlazorMeID.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

// Add authentication services
var scopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes");
string[] initialScopes = scopes!.Split(' ');

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddMicrosoftGraph("https://graph.microsoft.com/v1.0", scopes)
    .AddInMemoryTokenCaches();

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
builder.Services.AddScoped<HostingEnvironmentService>();
builder.Services.AddSingleton<BaseUrlProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddTransient<CookieHandler>()
    .AddScoped(sp => sp
        .GetRequiredService<IHttpClientFactory>()
        .CreateClient("API"))
    .AddHttpClient("API", (provider, client) =>
    {
        // Get base address
        var uri = provider.GetRequiredService<BaseUrlProvider>().BaseUrl;
        client.BaseAddress = new Uri(uri);
    }).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    //var policy = new AuthorizationPolicyBuilder()
    //    .RequireAuthenticatedUser()
    //    .Build();
    //options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSecurityHeaders(
    SecurityHeadersDefinitions.GetHeaderPolicyCollection(app.Environment.IsDevelopment(),
        app.Configuration["AzureAd:Instance"]));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
    await httpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme,
        new AuthenticationProperties
        {
            RedirectUri = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
        });
});

app.MapGet("/Account/Logout", async (HttpContext httpContext, string returnUrl = "/") =>
{
    var authenticationProperties = new AuthenticationProperties
    {
        RedirectUri = "/SignedOut"
    };
    if (httpContext.Request.Cookies.Count > 0)
    {
        var siteCookies = httpContext.Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
        foreach (var cookie in siteCookies)
        {
            httpContext.Response.Cookies.Delete(cookie.Key);
        }
    }

    await httpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}).RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();