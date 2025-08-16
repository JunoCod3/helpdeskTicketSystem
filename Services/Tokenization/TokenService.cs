using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FullstackDevTS.Models.Internal;
using FullstackDevTS.Services.Tokenization.Interface;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace FullstackDevTS.Services.Tokenization;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        this._configuration = configuration;

        if (string.IsNullOrEmpty(_configuration["JWT:Secret"]))
        {
            throw new ArgumentNullException("JWT:Secret",
                "JWT Secret key is not configured in appsettings.json");
        }
    }
    
    public string GenerateAccessToken(Users user)
    {
        
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(ClaimTypes.Name, user.Credential.Username),
          new Claim(ClaimTypes.Role, user.Information.UserLevel.ToString())
      };

      var token = new JwtSecurityToken(
          issuer: _configuration["JWT:ValidIssuer"],
          audience: _configuration["JWT:ValidAudience"],
          claims: claims,
          notBefore: DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JWT:TokenValidityInMinutes")),
          signingCredentials: credentials
          );
      
      return new JwtSecurityTokenHandler().WriteToken(token); 
      
    }

    public string GenerateRefreshToken(Users user)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {

        var tokenValidationParameter = new TokenValidationParameters
        {
            ValidateAudience = _configuration.GetValue<bool>("JWT:ValidateAudience"),
            ValidateIssuer = _configuration.GetValue<bool>("JWT:ValidateIssuer"),
            ValidateIssuerSigningKey = _configuration.GetValue<bool>("JWT:ValidateIssuerSigningKey"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)),
            ValidateLifetime = _configuration.GetValue<bool>("JWT:ValidateLifetime")
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out SecurityToken securityToken);


        if (securityToken is not JwtSecurityToken jwtSecurityToken
            ||  !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase) ) 
            throw new SecurityTokenException("Invalid token");
        
        return principal;
        
    }
}