using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Plans.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Plans.Queries
{
    public class GetPlanPagedListQuery : PagingOptions, IRequest<PagedResult<PlanDto>>
    {

    }

    public class GetPlanPagedListQueryHandler : IRequestHandler<GetPlanPagedListQuery, PagedResult<PlanDto>>
    {
        private readonly IMongoRepository<Plan> _planRepository;
        private readonly IMapper _mapper;

        public GetPlanPagedListQueryHandler(IMongoRepository<Plan> planRepository, IMapper mapper)
        {
            _planRepository = planRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<PlanDto>> Handle(GetPlanPagedListQuery request, CancellationToken cancellationToken)
        {
            var roles = _planRepository.GetAll(x => x.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(request.Query))
                roles = roles.Where(x => x.Name.Contains(request.Query));

            var planList = await _planRepository.BrowseAsync(roles, request);


            return _mapper.Map<PagedResult<PlanDto>>(planList);
        }
    }
}