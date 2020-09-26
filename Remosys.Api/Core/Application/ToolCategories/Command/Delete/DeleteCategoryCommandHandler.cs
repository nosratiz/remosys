using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.ToolCategories.Command.Delete
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly IMongoRepository<ToolsCategory> _toolCategoryRepository;
        private readonly IMongoRepository<Department> _departmentRepository;

        public DeleteCategoryCommandHandler(IMongoRepository<ToolsCategory> toolCategoryRepository, IMongoRepository<Department> departmentRepository)
        {
            _toolCategoryRepository = toolCategoryRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _toolCategoryRepository.GetAsync(request.Id, cancellationToken);

            if (category is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.CategoryNotFound)));

            if (await _departmentRepository
                .ExistsAsync(x => x.IsDeleted == false
                                  && x.ToolsCategories.Any(tc => tc.Id == request.Id), cancellationToken))
                return Result.Failed(new BadRequestObjectResult(
                    new ApiMessage(ResponseMessage.CategoryInDepartment)));



            await _toolCategoryRepository.DeleteAsync(request.Id);

            return Result.SuccessFul();
        }
    }
}