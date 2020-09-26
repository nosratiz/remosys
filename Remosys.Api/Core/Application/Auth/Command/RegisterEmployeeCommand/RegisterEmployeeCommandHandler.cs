using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Auth.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;
using Remosys.Common.Sms;

namespace Remosys.Api.Core.Application.Auth.Command.RegisterEmployeeCommand
{
    public class RegisterEmployeeCommandHandler : IRequestHandler<RegisterEmployeeCommand, Result<RegisterResult>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IPayamakService _payamakService;

        public RegisterEmployeeCommandHandler(IMongoRepository<User> userRepository, IPayamakService payamakService)
        {
            _userRepository = userRepository;
            _payamakService = payamakService;
        }

        public async Task<Result<RegisterResult>> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.Mobile == request.Mobile && x.IsDelete == false,
                cancellationToken);

            if (user is null)
                return Result<RegisterResult>
                    .Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            user.IsMobileConfirm = true;
            user.ExpiredCode = DateTime.Now.AddMinutes(2);

            await _userRepository.UpdateAsync(user);

            await _payamakService.SendMessage(user.Mobile, user.ActiveCode);

            return Result<RegisterResult>.SuccessFul(new RegisterResult { Mobile = user.Mobile });
        }
    }
}