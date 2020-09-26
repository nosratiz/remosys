using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Plans.Command.UpdatePlan
{
    public class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand, Result>
    {
        private readonly IMongoRepository<Plan> _planRepository;
        private readonly IMapper _mapper;

        public UpdatePlanCommandHandler(IMongoRepository<Plan> planRepository, IMapper mapper)
        {
            _planRepository = planRepository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _planRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id, cancellationToken);

            if (plan is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.PlanNotFound)));

            _mapper.Map(request, plan);

            await _planRepository.UpdateAsync(plan);

            return Result.SuccessFul();
        }
    }
}