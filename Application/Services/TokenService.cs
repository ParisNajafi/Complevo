using Core.Dtos.User;
using Core.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TokenService : ITokenService
	{
        private const double EXPIRY_DURATION_MINUTES = 30;
        private readonly IConfiguration iconfiguration;
		private readonly IAccountManager _accountManager;
		public TokenService(IConfiguration iconfiguration,IAccountManager accountManager)
		{
			this.iconfiguration = iconfiguration;
			_accountManager = accountManager;
		}
		public async Task<Tokens> Authenticate(LoginUserModel user)
		{
			if (!await _accountManager.IsValidUser(user.Username, user.Password))
				throw new ArgumentValidationException("Username or password is not correct");

            var token = BuildToken(user.Username);
            return new Tokens { Token = token };

        }

        private string BuildToken(string username)
        {
            var claims = new[] {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier,
            Guid.NewGuid().ToString())
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfiguration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(iconfiguration["Jwt:Issuer"], iconfiguration["Jwt:Audience"], claims,
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        public Task<bool> ValidateToken(string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(iconfiguration["Jwt:Key"]);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = iconfiguration["Jwt:Issuer"],
                    ValidAudience = iconfiguration["Jwt:Audience"],
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}
