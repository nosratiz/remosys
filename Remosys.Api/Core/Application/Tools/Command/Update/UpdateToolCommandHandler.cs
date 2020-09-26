using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tools.Command.Update
{
    public class UpdateToolCommandHandler : IRequestHandler<UpdateToolCommand, Result>
    {
        private readonly IMongoRepository<Tool> _toolRepository;
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMapper _mapper;

        public UpdateToolCommandHandler(IMongoRepository<Tool> toolRepository,
            IMongoRepository<ToolsCategory> toolCategoryRepository, IMapper mapper)
        {
            _toolRepository = toolRepository;
            _toolCategoryRepository = toolCategoryRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateToolCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var tool = await _toolRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id,
                cancellationToken);

            if (tool is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.ToolNotFound)));

            var category = await _toolCategoryRepository.GetAsync(request.CategoryId, cancellationToken);

            if (category is null)
                return Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.CategoryNotFound)));

            #endregion

            _mapper.Map(request, tool);

            tool.ToolsCategory = category;

            await _toolRepository.UpdateAsync(tool);

            return Result.SuccessFul();
        }
    }
}