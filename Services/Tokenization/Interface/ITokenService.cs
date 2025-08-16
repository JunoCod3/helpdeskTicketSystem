using System.Security.Claims;
using FullstackDevTS.Models.Internal;

namespace FullstackDevTS.Services.Tokenization.Interface;

public interface ITokenService
{
    string GenerateAccessToken(Users user);
    string GenerateRefreshToken(Users user);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

}