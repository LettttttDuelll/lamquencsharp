using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VNPT.Service
{
    public class JwtService
    {
        //tách từ api/token để có thể tái sử dụng nhiều lần

            private readonly Jwtsettings _jwtsettings;

            // Tiêm Jwtsettings vào đây để dùng chung cấu hình từ appsettings.json
            public JwtService(Jwtsettings jwtsettings)
            {
                _jwtsettings = jwtsettings;
            }

            public string GenerateToken(string username, string role)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role ?? "USER"), // Lưu quyền của user vào token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsettings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtsettings.Issuer,
                    audience: _jwtsettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_jwtsettings.ExpiryMinutes),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
}
