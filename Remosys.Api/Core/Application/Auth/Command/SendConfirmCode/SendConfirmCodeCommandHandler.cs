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
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IPayamakService _payamakService;

        public SendConfirmCodeCommandHandler(IMongoRepository<User> userRepository, IPayamakService payamakService, IMongoRepository<Employee> employeeRepository)
        {
            _userRepository = userRepository;
            _payamakService = payamakService;
            _employeeRepository = employeeRepository;
        }

        public async Task<Result> Handle(SendConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(x => x.Mobile == request.Mobile && x.IsDelete == false,
                cancellationToken);

            if (user is null)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            user.ExpiredCode = DateTime.Now.AddDays(2);
            user.ActiveCode = new Random().Next(10000, 99999).ToString();

            var employee = await _employeeRepository.GetAsync(x => x.IsDeleted == false && x.User.Id == user.Id, cancellationToken);

            //if user exist in employee consider as  approved 
            if (employee != null)
            {
                employee.IsApproved = true;
                await _employeeRepository.UpdateAsync(employee);
            }

            await _userRepository.UpdateAsync(user);

            await _payamakService.SendMessage(request.Mobile, user.ActiveCode);

            return Result.SuccessFul();
        }
    }
}