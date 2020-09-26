using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;
using Remosys.Common.Sms;

namespace Remosys.Api.Core.Application.Auth.Command.SendConfirmCode
{
    public class SendConfirmCodeCommandHandler : IRequestHandler<SendConfirmCodeCommand, Result>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IPayamakService _payamakService;

        public SendConfirmCodeCommandHandler(IMongoRepository<User> userRepository, IPayamakService payamakService)
        {
            _userRepository = userRepository;
            _payamakService = payamakService;
        }

        public async Task<Result> Handle(SendConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.Mobile == request.Mobile && x.IsDelete == false,
                cancellationToken);

            if (user is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            user.ExpiredCode = DateTime.Now.AddDays(2);
            user.ActiveCode = new Random().Next(10000, 99999).ToString();

            await _userRepository.UpdateAsync(user);

            await _payamakService.SendMessage(request.Mobile, user.ActiveCode);

            return Result.SuccessFul();
        }
    }
}