using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.ToolCategories.Command.Update
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IMongoRepository<ToolsCategory> toolCategoryRepository, IMapper mapper)
        {
            _toolCategoryRepository = toolCategoryRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _toolCategoryRepository.GetAsync(request.Id, cancellationToken);

            if (category is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.CategoryNotFound)));

            _mapper.Map(request, category);

            await _toolCategoryRepository.UpdateAsync(category);

            return Result.SuccessFul();
        }
    }
}