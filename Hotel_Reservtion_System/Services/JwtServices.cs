using Hotel_Reservtion_System.DTO;
using Hotel_Reservtion_System.Entity;
using Hotel_Reservtion_System.ServicesContracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hotel_Reservtion_System.Services
{
    public class JwtServices : IJwtServices
    {
        private readonly IConfiguration _configuration;
        public JwtServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string? GenerateToken(User user)
        {
            Claim[] claims =
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

                new Claim(ClaimTypes.Email,user.email),
                new Claim(ClaimTypes.Role,user.role)

            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                 _configuration["Jwt:Issuer"],
                 _configuration["Jwt:Audience"],
                 claims,
                 expires: DateTime.UtcNow.AddMinutes(20),
                 signingCredentials: creds
                 );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);
            return token;
        }
    }
}
