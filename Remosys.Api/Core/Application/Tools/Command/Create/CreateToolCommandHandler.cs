using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Tools.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tools.Command.Create
{
    public class CreateToolCommandHandler : IRequestHandler<CreateToolCommand, Result<ToolDto>>
    {
        private readonly IMongoRepository<Tool> _toolRepository;
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMapper _mapper;

        public CreateToolCommandHandler(IMongoRepository<Tool> toolRepository, IMapper mapper,
            IMongoRepository<ToolsCategory> toolCategoryRepository)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
            _toolCategoryRepository = toolCategoryRepository;
        }

        public async Task<Result<ToolDto>> Handle(CreateToolCommand request, CancellationToken cancellationToken)
        {
            var category = await _toolCategoryRepository.GetAsync(request.CategoryId, cancellationToken);

            if (category is null)
                return Result<ToolDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.CategoryNotFound)));

            var tool = _mapper.Map<Tool>(request);

            tool.ToolsCategory = category;

            await _toolRepository.AddAsync(tool);

            return Result<ToolDto>.SuccessFul(_mapper.Map<ToolDto>(tool));
        }
    }
}