using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Infrastructure.Data;

namespace Warehouse.API.Security
{
    public class JWTProvider
    {
        private readonly WarehouseDBContext _context;
        readonly IConfiguration _configuration;

        public JWTProvider(WarehouseDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GetToken(string userName, string password)
        {
            var user = _context.User.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());            
            
            if (user == null)
                return null;

            
            if (user.Password == password)
            {
                var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Security:SecurityKey"));
                var claims = new List<Claim>();
                var userRole = (Enumerations.Roles)user.Role;

                claims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));

                var JWToken = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("Security:ValidIssuer"),
                    audience: _configuration.GetValue<string>("Security:ValidAudience"),
                    claims: claims,
                    expires: new DateTimeOffset(DateTime.Now.AddHours(1)).DateTime,                    
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);

                return token;
            }
            else
            {
                return null;
            }
        }
    }
}
