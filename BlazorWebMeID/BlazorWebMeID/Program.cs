using BlazorWebMeID;
using BlazorWebMeID.Client.Pages;
using BlazorWebMeID.Components;
using BlazorWebMeID.Identity;
using BlazorWebMeID.Identity.Client.Services;
using BlazorWebMeID.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "__Host-X-XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

var scopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes");
string[] initialScopes = scopes!.Split(' ');

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddMicrosoftGraph("https://graph.microsoft.com/v1.0", scopes)
    .AddInMemoryTokenCaches();

// Without, the client component using the API as an error in server mode
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();

builder.Services.AddScoped<HostingEnvironmentService>();
builder.Services.AddSingleton<BaseUrlProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddScoped(sp => sp
        .GetRequiredService<IHttpClientFactory>()
        .CreateClient("API"))
    .AddHttpClient("API", (provider, client) =>
    {
        // Get base address
        var uri = provider.GetRequiredService<BaseUrlProvider>().BaseUrl;
        client.BaseAddress = new Uri(uri);
    });

builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    //var policy = new AuthorizationPolicyBuilder()
    //    .RequireAuthenticatedUser()
    //    .Build();
    //options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseSecurityHeaders(
    SecurityHeadersDefinitions.GetHeaderPolicyCollection(app.Environment.IsDevelopment(),
        app.Configuration["AzureAd:Instance"]));

app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();

AuthenticationExtensions.SetupEndpoints(app);

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();