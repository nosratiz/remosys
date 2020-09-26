using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Employees.Queries
{
    public class GetEmployeeQuery : IRequest<Result<EmployeeDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, Result<EmployeeDto>>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeQueryHandler(IMongoRepository<Employee> employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<Result<EmployeeDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetAsync(request.Id, cancellationToken);

            if (employee is null)
                return Result<EmployeeDto>.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

            return Result<EmployeeDto>.SuccessFul(_mapper.Map<EmployeeDto>(employee));
        }
    }
}