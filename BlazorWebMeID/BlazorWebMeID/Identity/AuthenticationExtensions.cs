using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace BlazorWebMeID.Identity;

public static class AuthenticationExtensions
{
    public static WebApplication SetupEndpoints(this WebApplication app)
    {
        app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
        {
            await httpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/"
                });
        });

        app.MapPost("/Account/Logout", async (HttpContext httpContext) =>
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "/SignedOut"
            };

            await httpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme,
                authenticationProperties);

            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }).RequireAuthorization();

        return app;
    }
}