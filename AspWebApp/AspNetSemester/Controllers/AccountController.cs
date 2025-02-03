// Controllers/AccountController.cs

using System.Security.Claims;
using System.Security.Cryptography;
using AspNetSemester.Models;
using AspNetSemester.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AspNetSemester.Repositories.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AspNetSemester.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("/account/register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("/account/register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _userRepository.GetUserByUsername(model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return View(model);
            }

            var hashPassword = Convert.ToHexString(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(model.Password)));
            var user = new User
            {
                Username = model.Username,
                Password = hashPassword
            };

            _userRepository.AddUser(user);

            return RedirectToAction("Login", "Account");
        }

        return View(model);
    }
    [HttpGet("/account/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("/account/login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (_userRepository.ValidateUser(model.Username, model.Password))
            {
                var user = _userRepository.GetUserByUsername(model.Username);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("UserId", user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = false 
                });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    
    [HttpPost("/account/logout")]
    [ValidateAntiForgeryToken] // Защита от CSRF-атак
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Cookies.Delete(".AspNetCore.Cookies"); // Удаление куки вручную
        return RedirectToAction("Login", "Account");
    }


    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}



