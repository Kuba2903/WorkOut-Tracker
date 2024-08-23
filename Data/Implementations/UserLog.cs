using Data.DTO_s;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Data.Implementations
{
    public class UserLog : IUserLog
    {
        private readonly AppDbContext _dbContext;
        private readonly IOptions<JwtSection> jwt;

        public UserLog(AppDbContext dbContext, IOptions<JwtSection> jwt)
        {
            _dbContext = dbContext;
            this.jwt = jwt;
        }

        public async Task<string> LoginAsync(LoginDTO login)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

            string token = GenerateToken(user);

            return token;
        }

        public async Task<string> RegisterAsync(RegisterDTO register)
        {
            if(register.Password != register.PasswordConfirm)
            {
                throw new Exception("passwords doesnt match");
            }

            var user = new User()
            {
                Email = register.Email,
                Name = register.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password)
            };

            

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return "Account created";
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var token = new JwtSecurityToken(issuer: jwt.Value.Issuer, audience: jwt.Value.Audience,
                claims: userClaims, expires: DateTime.Now.AddMinutes(2), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
    
    }
}
