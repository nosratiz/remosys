using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Command.Update
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IMongoRepository<Employee> employeeRepository,
            IMongoRepository<User> userRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            #region validation

            var employee = await _employeeRepository.GetAsync(request.Id, cancellationToken);

            if (employee is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

            var user = await _userRepository.GetAsync(x => x.IsDelete == false && x.Id == request.UserId,
                cancellationToken);

            if (user is null)
                return Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));
  

            #endregion
          
            _mapper.Map(request, employee);

            employee.User = user;

            await _employeeRepository.UpdateAsync(employee);

            return Result.SuccessFul();
        }
    }
}