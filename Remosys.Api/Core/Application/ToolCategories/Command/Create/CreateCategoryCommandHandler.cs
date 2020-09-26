using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.ToolCategories.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.ToolCategories.Command.Create
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<ToolCategoryDto>>
    {
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IMongoRepository<ToolsCategory> toolCategoryRepository, IMapper mapper)
        {
            _toolCategoryRepository = toolCategoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<ToolCategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var toolCategory = _mapper.Map<ToolsCategory>(request);

            await _toolCategoryRepository.AddAsync(toolCategory);

            return Result<ToolCategoryDto>.SuccessFul(_mapper.Map<ToolCategoryDto>(toolCategory));
        }
    }
}