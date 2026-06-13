using Microsoft.IdentityModel.Tokens;
using MOFU.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MOFU.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Users users)
        {
            var key =_config["Jwt:Key"];
            var issuer=_config["Jwt:Issuer"];
            var audience=_config["Jwt:Audience"];
            var expireMinutes =int.Parse(_config["Jwt:ExpireMinutes"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, users.UserId.ToString()),
                new Claim(ClaimTypes.Email, users.UserEmail),
                new Claim(ClaimTypes.Name, users.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                
            };

            var encodeKey=Encoding.UTF8.GetBytes(key);

            //使用HMACSHA256算法生成簽名
            var securityKey = new SymmetricSecurityKey(encodeKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires:DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials : credentials
                );

            //物件序列為JWT格式的字符串
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    }

