using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Command.Delete
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;

        public DeleteEmployeeCommandHandler(IMongoRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Result> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetAsync(request.Id, cancellationToken);

            if (employee is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

            employee.IsDeleted = true;

            await _employeeRepository.UpdateAsync(employee);

            return Result.SuccessFul();
        }
    }
}