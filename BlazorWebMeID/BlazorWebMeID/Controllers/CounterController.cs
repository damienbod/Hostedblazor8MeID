using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebMeID.Controllers;

// [ValidateAntiForgeryToken] not working yet, but this is required, or the custom header CRSF protection
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class CounterController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Data from secure API";
    }
}
