using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.AgentSetting.Command.Update
{
    public class UpdateAgentSettingCommandHandler : IRequestHandler<UpdateAgentSettingCommand, Result>
    {
        private readonly IMongoRepository<Models.AgentSetting> _agentSettingRepository;
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public UpdateAgentSettingCommandHandler(IMongoRepository<Models.AgentSetting> agentSettingRepository,
            IMongoRepository<Employee> employeeRepository, IMapper mapper)
        {
            _agentSettingRepository = agentSettingRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateAgentSettingCommand request, CancellationToken cancellationToken)
        {
            var agentSetting =
                await _agentSettingRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id,
                    cancellationToken);

            if (agentSetting is null)
                return Result.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.SettingNotFound)));

            _mapper.Map(request, agentSetting);

            List<Employee> employees = new List<Employee>();
            foreach (var id in request.EmployeeIds)
            {
                var employee = await _employeeRepository.GetAsync(id, cancellationToken);

                if (employee is null)
                    return Result.Failed(
                        new NotFoundObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

                employees.Add(employee);
            }

            agentSetting.Employees = employees;

            await _agentSettingRepository.UpdateAsync(agentSetting);

            return Result.SuccessFul();

        }
    }
}