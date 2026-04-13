using BoschPizza.Data;
using BoschPizza.Models;
using BoschPizza.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoschPizza.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly TokenService _tokenService;
    private readonly AppDbContext _context;

    // Metodo construtor
    public AuthController(IConfiguration configuration, TokenService tokenService, AppDbContext context)
    {
        _configuration = configuration;
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login(UserLogin login)
    {
        var user = _context.UserLogins.FirstOrDefault(u => u.Username == login.Username);

        if (user == null)
        {
            return Unauthorized(new { message = "Usuário ou senha inválidos"});
        }

        if (user.Password != login.Password)
        {
            return Unauthorized(new { message = "Senha inválida"});
        }

        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;

        var token = _tokenService.GenerateToken(login.Username, key, issuer, audience);

        return Ok(new { token });
    }
}

