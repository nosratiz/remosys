using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.AgentSetting.Command.Create
{
    public class CreateAgentSettingCommandHandler : IRequestHandler<CreateAgentSettingCommand, Result>
    {
        private readonly IMongoRepository<Models.AgentSetting> _agentSettingRepository;
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public CreateAgentSettingCommandHandler(IMongoRepository<Models.AgentSetting> agentSettingRepository, IMapper mapper, ICurrentUserService currentUserService, IMongoRepository<Employee> employeeRepository)
        {
            _agentSettingRepository = agentSettingRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        public async Task<Result> Handle(CreateAgentSettingCommand request, CancellationToken cancellationToken)
        {
            List<Employee> employees = new List<Employee>();
        
            foreach (var id in request.EmployeeIds)
            {
                var employee = await _employeeRepository.GetAsync(id, cancellationToken);

                if (employee is null)
                    return Result.Failed(
                        new NotFoundObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

                employees.Add(employee);
            }

            var agentSetting = _mapper.Map<Models.AgentSetting>(request);
            agentSetting.Employees = employees;
          
            await _agentSettingRepository.AddAsync(agentSetting);

            return Result.SuccessFul();
        }
    }
}