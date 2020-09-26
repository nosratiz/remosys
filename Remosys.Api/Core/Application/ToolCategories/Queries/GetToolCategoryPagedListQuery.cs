using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.ToolCategories.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.ToolCategories.Queries
{
    public class GetToolCategoryPagedListQuery : PagingOptions, IRequest<PagedResult<ToolCategoryDto>>
    {

    }


    public class GetToolCategoryPagedListQueryHandler : IRequestHandler<GetToolCategoryPagedListQuery, PagedResult<ToolCategoryDto>>
    {
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMapper _mapper;

        public GetToolCategoryPagedListQueryHandler(IMongoRepository<ToolsCategory> toolCategoryRepository, IMapper mapper)
        {
            _toolCategoryRepository = toolCategoryRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ToolCategoryDto>> Handle(GetToolCategoryPagedListQuery request, CancellationToken cancellationToken)
        {
            var categories = _toolCategoryRepository.GetAll().Where(x=>x.IsDeleted==false);

            if (!string.IsNullOrWhiteSpace(request.Query))
                categories = categories.Where(x => x.Name.Contains(request.Query));


            var categoryList =await _toolCategoryRepository.BrowseAsync(categories, request);

            return _mapper.Map<PagedResult<ToolCategoryDto>>(categoryList);
        }
    }
}