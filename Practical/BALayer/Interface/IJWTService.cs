using DALayer.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.Interface
{
    public interface IJWTService
    {
        string GenerateJwtToken(EmployeeModel employee);

        bool Validate(string jwtToken, out JwtSecurityToken jwtSecurityToken, out IEnumerable<Claim> claims);
        public int GetUserIdFromToken(string jwtToken);
    }
}
