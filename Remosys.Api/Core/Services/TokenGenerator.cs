using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Api.Core.Interfaces;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper;
using Remosys.Common.Mongo;
using Remosys.Common.Options;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IMongoRepository<UserToken> _userTokenRepository;
        private readonly IRequestMeta _requestMeta;

        public TokenGenerator(IOptions<JwtSettings> jwtSettings, IRequestMeta requestMeta, IMongoRepository<UserToken> userTokenRepository)
        {
            _requestMeta = requestMeta;
            _userTokenRepository = userTokenRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Result<AuthenticationResult>> Generate(User user, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("fullName",$"{user.FirstName} {user.LastName}"),
                new Claim("RoleName",user.Roles.FirstOrDefault()!.Name)

            };

            claims.AddRange(user.Roles.Select(role =>
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
            ));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(14),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtSettings.ValidAudience,
                Issuer = _jwtSettings.ValidIssuer
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userToken = new UserToken
            {
                Token = tokenHandler.WriteToken(token),
                CreateDate = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow.AddDays(14),
                Device = _requestMeta.Device,
                Os = _requestMeta.Os,
                Ip = _requestMeta.Ip,
                UserAgent = _requestMeta.UserAgent,
                UserId = user.Id
            };

            await _userTokenRepository.AddAsync(userToken);


            return Result<AuthenticationResult>.SuccessFul(new AuthenticationResult
            {
                AccessToken = userToken.Token,
                IsSuccess = true,
                RoleName = user.Roles.FirstOrDefault()?.Name
            });
        }
    }
}