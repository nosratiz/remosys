using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Plans.Command.DeletePlan
{
    public class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand, Result>
    {
        private readonly IMongoRepository<Plan> _planRepository;

        public DeletePlanCommandHandler(IMongoRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<Result> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _planRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id, cancellationToken);

            if (plan is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.PlanNotFound)));

            plan.IsDeleted = false;

            await _planRepository.UpdateAsync(plan);

            return Result.SuccessFul();
        }
    }
}