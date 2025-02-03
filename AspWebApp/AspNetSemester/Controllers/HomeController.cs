using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSemester.Controllers;

[Authorize]
public class HomeController : Controller
{
    [HttpGet("/home")]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    [HttpGet("/document")]
    public IActionResult Document()
    {
        return View();
    }

    [Authorize]
    [HttpGet("/documentlist")]
    public IActionResult ListOfDocuments()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
        Console.WriteLine(userIdClaim.Value);
        return View();
    }
}