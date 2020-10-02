using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Command.Activate
{
    public class ActivateEmployeeCommandHandler : IRequestHandler<ActivationEmployeeCommand, Result>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMongoRepository<Contract> _contractRepository;
        private readonly ICurrentUserService _currentUserService;

        public ActivateEmployeeCommandHandler(IMongoRepository<Employee> employeeRepository, IMongoRepository<Contract> contractRepository, ICurrentUserService currentUserService)
        {
            _employeeRepository = employeeRepository;
            _contractRepository = contractRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(ActivationEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetAsync(x => x.Id == request.EmployeeId && x.IsDeleted == false,
                cancellationToken);

            if (employee is null)
                return Result.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

            if (employee.IsActive == false)
            {
                var contract =
                    await _contractRepository.GetAsync(x => x.User.Id == Guid.Parse(_currentUserService.UserId) && x.EndContract > DateTime.Now, cancellationToken);

                if (contract is null)
                    return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.ExpiredContract)));
                
                int activeEmploye = _employeeRepository.GetAll().Count(x => x.IsDeleted == false && x.IsActive);

                if (contract.Plan.PersonCount < activeEmploye + 1)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.ActiveEmployeeReach)));

            }

            employee.IsActive = !employee.IsActive;

            await _employeeRepository.UpdateAsync(employee);

            return Result.SuccessFul();
        }
    }
}