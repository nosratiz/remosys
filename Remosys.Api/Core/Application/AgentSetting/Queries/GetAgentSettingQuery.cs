using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.AgentSetting.Dto;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.AgentSetting.Queries
{
    public class GetAgentSettingQuery : IRequest<Result<AgentSettingDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetAgentSettingQueryHandler : IRequestHandler<GetAgentSettingQuery, Result<AgentSettingDto>>
    {
        private readonly IMongoRepository<Models.AgentSetting> _agentSettingRepository;
        private readonly IMapper _mapper;

        public GetAgentSettingQueryHandler(IMongoRepository<Models.AgentSetting> agentSettingRepository, IMapper mapper)
        {
            _agentSettingRepository = agentSettingRepository;
            _mapper = mapper;
        }

        public async Task<Result<AgentSettingDto>> Handle(GetAgentSettingQuery request, CancellationToken cancellationToken)
        {
            var agentSetting = await _agentSettingRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id, cancellationToken);

            if (agentSetting is null)
                return Result<AgentSettingDto>.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.SettingNotFound)));


            return Result<AgentSettingDto>.SuccessFul(_mapper.Map<AgentSettingDto>(agentSetting));
        }
    }
}