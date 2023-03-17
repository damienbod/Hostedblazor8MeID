using HostedBlazorAad.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostedBlazorAad.Server.Controllers;

[ValidateAntiForgeryToken]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class DirectApiController : ControllerBase
{
    [HttpGet]
    public IEnumerable<MyGridData> Get()
    {
        var data = Random.Shared.GetItems(MyData.GetData(), 10);
        return data;
    }
}
