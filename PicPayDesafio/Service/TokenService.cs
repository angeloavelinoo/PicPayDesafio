using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PicPayDesafio.Configs;
using PicPayDesafio.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PicPayDesafio.Service
{
    public class TokenService
    {

        public static string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration.SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, usuario.Email),
                    new(ClaimTypes.Role, usuario.Role),
                    new("id", usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token?.Split(" ").Last()) as JwtSecurityToken;

            // Verifica se o token não é nulo e se tem a reivindicação (claim) de ID do usuário
            if (jwtToken == null || !jwtToken.Claims.Any(c => c.Type == "id"))
            {
                throw new ArgumentException("Token inválido ou sem ID do usuário.");
            }

            // Extrai o ID do usuário do token
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new ArgumentException("ID do usuário ausente ou inválido no token.");
            }

            return userId;
        }
    }
}