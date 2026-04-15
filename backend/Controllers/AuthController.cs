using BoschPizza.Data;
using BoschPizza.Models;
using BoschPizza.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
        // substituir login fixo por banco de dados
        var user = _context.UserLogins
            .FirstOrDefault(u => u.Username == login.Username);

        if (user == null)
        {
            return Unauthorized(new { message = "Usuário ou senha inválidos" });
        }

        var passwordHasher = new PasswordHasher<UserLogin>();

        var result = passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            login.Password
        );

        if (result == PasswordVerificationResult.Failed)
        {   
            return Unauthorized(new { message = "Usuário ou senha inválidos" });
        }

        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;

        var token = _tokenService.GenerateToken(
            user.Username,
            user.Role,
            key,
            issuer,
            audience
        );

        return Ok(new { token });
    }

    // Registra usuários no banco
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserLogin newUser)
    {
        var exists = await _context.UserLogins
            .AnyAsync(u => u.Username == newUser.Username);

        if (exists)
        {
            return BadRequest(new { message = "Usuário já existe" });
        }

        var passwordHasher = new PasswordHasher<UserLogin>();

        newUser.Password = passwordHasher.HashPassword(newUser, newUser.Password);

        newUser.Role = string.IsNullOrEmpty(newUser.Role) ? "User" : newUser.Role;
        
        _context.UserLogins.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Usuário cadastrado com sucesso" });
    }

    // Listar usuários
    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    [Authorize] // coisas do chatgpt
    public async Task<ActionResult<List<UserLogin>>> GetUsers()
    {
        var users = await _context.UserLogins
            .Select(u => new 
            {
                u.Id,
                u.Username
            })
            .ToListAsync();

        return Ok(users);
    }
    
    // Deletar usuários
    [Authorize(Roles = "Admin")]
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.UserLogins.FindAsync(id);

        if (user == null)
            return NotFound();

        _context.UserLogins.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

