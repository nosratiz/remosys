using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Api.Core.Interfaces;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Auth.Command.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenDto>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<UserToken> _userTokenRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginCommandHandler(IMongoRepository<User> userRepository, ITokenGenerator tokenGenerator, IMongoRepository<UserToken> userTokenRepository)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<Result<TokenDto>> Handle(Command.LoginCommand.LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.Mobile == request.Mobile && x.IsDelete == false, cancellationToken);

            if (user is null)
                return Result<TokenDto>.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.InvalidUserNameOrPassword)));

            if (PasswordManagement.CheckPassword(request.Password, user.Password) == false)
                return Result<TokenDto>.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.InvalidUserNameOrPassword)));

            if (user.IsMobileConfirm == false)
                return Result<TokenDto>.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.AccountDeactivate)));


            var userToken = await _userTokenRepository.GetAsync(x =>
                x.IsExpired == false && x.ExpiredDate >= DateTime.Now && x.UserId == user.Id, cancellationToken);

            //if user already have valid token in database
            if (userToken != null)
            {
                return Result<TokenDto>.SuccessFul(new TokenDto
                {
                    AccessToken = userToken.Token
                });
            }

            var result = await _tokenGenerator.Generate(user, cancellationToken);

            return Result<TokenDto>.SuccessFul(new TokenDto
            {
                AccessToken = result.Data.AccessToken,
                RoleName = result.Data.RoleName
            });
        }
    }
}