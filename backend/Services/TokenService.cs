using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BoschPizza.Services;

public class TokenService
{
    public string GenerateToken(string username, string role, string key, string issuer, string audience)
    {  
        //Cria as claims que serão colocados dentro do token
        var claims = new[] 
        { 
            // Armazena o nome do usuario dentro do token
            new Claim(ClaimTypes.Name, username) ,
            new Claim(ClaimTypes.Role, role) // ainda não sei o que faz
        };

        // Criar a chave de segurança com base no segredo configurado no servidor
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        // Definir a credencial de assinatura usando algoritmo HMAC SHA256
        var credencial = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Cria o token JWT com emissor, audiencia, claims, expiração e assinatura
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credencial
        );

        // Converte o objeto token em string para ser enviada ao cliente
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}