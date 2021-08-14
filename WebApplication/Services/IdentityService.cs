using AowCore.Domain;
using API.Models;
using API.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthResult
                {
                    ErrorMessages = new[] { "User does not exist" }
                };

            };
           
            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!checkPassword)
            {
                return new AuthResult
                {
                    ErrorMessages = new[] { "Not Valid Password" }
                };
            }

            return GenerateTokenForUserAuthResult(user);
        }

        public async Task<AuthResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthResult
                {
                    ErrorMessages = new[] { "User Allready Exist" }
                };

            };

            var newUser = new ApplicationUser
            {
                Email = email,
                UserName = email
            };
            var createUser = await _userManager.CreateAsync(newUser, password);
            if (!createUser.Succeeded)
            {
                return new AuthResult
                {
                    ErrorMessages = createUser.Errors.Select(x => x.Description)
                };
            }

            return GenerateTokenForUserAuthResult(newUser);
        }

        private AuthResult GenerateTokenForUserAuthResult(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Id", user.Id)
                }),

                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            //token.Header.Add("kid", "");
            //token.Payload.Remove("iss");
            //token.Payload.Add("iss", "your issuer");

            var tokenString = tokenHandler.WriteToken(token);

            return new AuthResult
            {
                Success = true,
                Token = tokenString,
            };
        }
    }
}





