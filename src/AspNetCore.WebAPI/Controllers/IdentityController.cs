using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AspNetCore.WebAPI.Controllers;

[Route("webapi")]
[Authorize]
public class IdentityController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
    }
}