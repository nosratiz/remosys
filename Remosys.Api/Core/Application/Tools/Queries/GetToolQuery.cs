using System;
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

namespace Remosys.Api.Core.Application.Tools.Queries
{
    public class GetToolQuery : IRequest<Result<ToolDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetToolQueryHandler : IRequestHandler<GetToolQuery, Result<ToolDto>>
    {
        private readonly IMongoRepository<Tool> _toolRepository;
        private readonly IMapper _mapper;

        public GetToolQueryHandler(IMongoRepository<Tool> toolRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<Result<ToolDto>> Handle(GetToolQuery request, CancellationToken cancellationToken)
        {
            var tool = await _toolRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id,
                cancellationToken);

            if (tool is null)
                return Result<ToolDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.ToolNotFound)));

               return Result<ToolDto>.SuccessFul(_mapper.Map<ToolDto>(tool));
        }
    }
}