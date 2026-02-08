using Microsoft.AspNetCore.Mvc;

namespace WhoopNet.WhoopWhoopApp.Controllers;

[ApiController]
[Route("oauth")]
public class OauthController : ControllerBase
{
    [HttpGet(Name = "redirect")]
    public IActionResult OAuthRedirect([FromBody] object oauthResponse)
    {
        return Redirect("/");
    }
}
