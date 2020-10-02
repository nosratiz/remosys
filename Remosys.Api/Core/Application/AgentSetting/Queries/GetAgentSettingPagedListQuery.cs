using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.AgentSetting.Dto;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.AgentSetting.Queries
{
    public class GetAgentSettingPagedListQuery : PagingOptions, IRequest<PagedResult<AgentSettingListDto>>
    {

    }


    public class GetAgentSettingPagedListQueryHandler : IRequestHandler<GetAgentSettingPagedListQuery, PagedResult<AgentSettingListDto>>
    {
        private readonly IMongoRepository<Models.AgentSetting> _agentSettingRepository;
        private readonly IMapper _mapper;

        public GetAgentSettingPagedListQueryHandler(IMongoRepository<Models.AgentSetting> agentSettingRepository, IMapper mapper)
        {
            _agentSettingRepository = agentSettingRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<AgentSettingListDto>> Handle(GetAgentSettingPagedListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Models.AgentSetting> agentSettings =
                _agentSettingRepository.GetAll().Where(x => x.IsDeleted == false);


            if (!string.IsNullOrWhiteSpace(request.Query))
                agentSettings = agentSettings.Where(x => x.Name.Contains(request.Query));

            var agentSettingList = await _agentSettingRepository.BrowseAsync(agentSettings, request);

            return _mapper.Map<PagedResult<AgentSettingListDto>>(agentSettingList);
        }
    }
}