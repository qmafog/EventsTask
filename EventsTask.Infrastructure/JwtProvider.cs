using EventsTask.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using EventsTask.Application.Interfaces;
using Microsoft.Extensions.Primitives;
using EventsTask.Application.Common.Dtos;
using System.Security.Cryptography;


namespace EventsTask.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IUserRepository _userRepository;
        public JwtProvider(IOptions<JwtOptions> jwtOptions, IUserRepository userRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _userRepository = userRepository;
        }
        public TokenDto GenerateToken(Guid userId, bool isAdmin, bool populateExp)
        {
            Claim[] claims = [new("UserId", userId.ToString()),
                              new(ClaimsIdentity.DefaultRoleClaimType, isAdmin ? "Admin": "User")
                             ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.Expires));

            string tokenValue = string.Empty;
            try
            {
                tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var refreshToken = GenerateRefreshToken();
            DateTime? refreshExp = null;
            if (populateExp)
            {
                refreshExp = DateTime.UtcNow.AddDays(7);
            }

            _userRepository.UpdateRefreshToken(userId, refreshToken, refreshExp);

            return new TokenDto(tokenValue, refreshToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        public bool ValidateToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            return jwtSecurityToken != null &&
                jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<TokenDto?> RefreshToken(TokenDto tokenDto)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenDto.AccessToken);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var userId = Guid.Parse(userIdClaim);
            var user = await _userRepository.GetById(userId);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            var userRoles = await _userRepository.GetUserRoles(user.Id);

            bool isAdmin = userRoles.Contains(Domain.Enums.Role.Admin);

            return GenerateToken(user.Id, isAdmin, false);
        }
    }
}
