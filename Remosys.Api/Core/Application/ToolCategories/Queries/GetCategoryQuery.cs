using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.ToolCategories.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.ToolCategories.Queries
{
    public class GetCategoryQuery : IRequest<Result<ToolCategoryDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Result<ToolCategoryDto>>
    {
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(IMongoRepository<ToolsCategory> toolCategoryRepository, IMapper mapper)
        {
            _toolCategoryRepository = toolCategoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<ToolCategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _toolCategoryRepository.GetAsync(request.Id, cancellationToken);

            if (category is null)
                return Result<ToolCategoryDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.CategoryNotFound)));


            return Result<ToolCategoryDto>.SuccessFul(_mapper.Map<ToolCategoryDto>(category));
        }
    }
}