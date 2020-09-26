using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tools.Command.Delete
{
    public class DeleteToolCommandHandler : IRequestHandler<DeleteToolCommand, Result>
    {
        private readonly IMongoRepository<Tool> _toolRepository;

        public DeleteToolCommandHandler(IMongoRepository<Tool> toolRepository)
        {
            _toolRepository = toolRepository;
        }

        public async Task<Result> Handle(DeleteToolCommand request, CancellationToken cancellationToken)
        {
            var tool = await _toolRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id,
                cancellationToken);

            if (tool is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.ToolNotFound)));

            tool.IsDeleted = true;
            
            await _toolRepository.UpdateAsync(tool);

            return Result.SuccessFul();

        }
    }
}