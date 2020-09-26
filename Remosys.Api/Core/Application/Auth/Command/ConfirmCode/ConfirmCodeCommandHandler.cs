using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Api.Core.Interfaces;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Auth.Command.ConfirmCode
{
    public class ConfirmCodeCommandHandler : IRequestHandler<ConfirmCodeCommand, Result<TokenDto>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly ITokenGenerator _tokenGenerator;

        public ConfirmCodeCommandHandler(IMongoRepository<User> userRepository, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<Result<TokenDto>> Handle(ConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.Mobile == request.Mobile && x.ActiveCode == request.Code,
                cancellationToken);

            if (user is null)
                return Result<TokenDto>.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.InvalidConfirmCode)));

            if (user.ExpiredCode < DateTime.Now)
                return Result<TokenDto>.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.ExpiredCode)));

            user.IsMobileConfirm = true;

            await _userRepository.UpdateAsync(user);

            var result = await _tokenGenerator.Generate(user, cancellationToken);

            return Result<TokenDto>.SuccessFul(new TokenDto
            {
                AccessToken = result.Data.AccessToken
            });
        }
    }
}