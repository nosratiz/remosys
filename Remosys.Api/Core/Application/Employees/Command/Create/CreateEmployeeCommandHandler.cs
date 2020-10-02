using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Employees.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;
using Remosys.Common.Sms;

namespace Remosys.Api.Core.Application.Employees.Command.Create
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<EmployeeDto>>
    {
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMongoRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMongoRepository<Role> _roleRepository;
        private readonly IPayamakService _payamakService;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IMongoRepository<Employee> employeeRepository, IMapper mapper,
            IMongoRepository<User> userRepository, ICurrentUserService currentUserService, IMongoRepository<Role> roleRepository, IPayamakService payamakService)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _roleRepository = roleRepository;
            _payamakService = payamakService;
        }

        public async Task<Result<EmployeeDto>> Handle(CreateEmployeeCommand request,
            CancellationToken cancellationToken)
        {

            var currentUser = await _userRepository.GetAsync(u => u.Id == Guid.Parse(_currentUserService.UserId),
                cancellationToken);

            if (currentUser.Organizations is null)
                return Result<EmployeeDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.NeedOrganization)));

            #region Create User First

            var user = _mapper.Map<User>(request);

            var role = await _roleRepository.GetAsync(x => x.Name == Role.User, cancellationToken);
            user.Roles = new List<Role> { role };

            user.Organizations = new List<Models.Organization> { currentUser.Organizations.FirstOrDefault() };

            await _userRepository.AddAsync(user);

            #endregion
            await _payamakService.SendInvitation(request.Mobile, currentUser.Organizations.FirstOrDefault()?.Name, "");

            var employee = _mapper.Map<Employee>(request);

            employee.User = user;
            employee.Organization = currentUser.Organizations.FirstOrDefault();
        
            await _employeeRepository.AddAsync(employee);

            return Result<EmployeeDto>.SuccessFul(_mapper.Map<EmployeeDto>(employee));
        }
    }
}