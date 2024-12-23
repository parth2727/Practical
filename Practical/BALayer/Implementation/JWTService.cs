using BALayer.Interface;
using DALayer.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.Implementation
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration configuration;

        public JWTService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateJwtToken(EmployeeModel employee)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("EmployeeId",employee.EmployeeId.ToString()),
                new Claim("IsAdmin",employee.IsAdmin.ToString()),
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Convert.ToString(configuration["Jwt:Key"])));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires =
                DateTime.Now.AddMinutes(60);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool Validate(string jwtToken, out JwtSecurityToken jwtSecurityToken, out IEnumerable<Claim> claims)
        {
            jwtSecurityToken = null;
            claims = null;
            if (jwtToken == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Convert.ToString(configuration["Jwt:Key"]));

            try
            {
                tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;

                // Extract claims if the token is valid
                claims = jwtSecurityToken.Claims;
                return jwtSecurityToken.ValidTo >= DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
        public  int GetUserIdFromToken(string jwtToken)
        {
            if (string.IsNullOrWhiteSpace(jwtToken))
                throw new ArgumentException("JWT token cannot be null or empty");

            var handler = new JwtSecurityTokenHandler();

            // Check if the token is in a valid format
            if (!handler.CanReadToken(jwtToken))
                throw new ArgumentException("Invalid JWT token");

            var token = handler.ReadJwtToken(jwtToken);

            // Extract UserId claim (ensure the claim key matches the one in your token)
            var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "UserId");
            if (userIdClaim == null)
                throw new Exception("UserId claim not found in JWT token");

            return Convert.ToInt32(userIdClaim.Value);
        }

    }
}
