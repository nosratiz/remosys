using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Tools.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Tools.Queries
{
    public class GetToolPagedListQuery : PagingOptions, IRequest<PagedResult<ToolDto>>
    {
    }

    public class GetToolPagedListQueryHandler : IRequestHandler<GetToolPagedListQuery, PagedResult<ToolDto>>
    {
        private readonly IMongoRepository<Tool> _toolRepository;
        private readonly IMapper _mapper;

        public GetToolPagedListQueryHandler(IMongoRepository<Tool> toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ToolDto>> Handle(GetToolPagedListQuery request,
            CancellationToken cancellationToken)
        {
            var tools = _toolRepository.GetAll().Where(x => x.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(request.Query))
                tools = tools.Where(x => x.Name.Contains(request.Query));

            var toolList = _toolRepository.BrowseAsync(tools, request);

            return _mapper.Map<PagedResult<ToolDto>>(toolList);
        }
    }
}