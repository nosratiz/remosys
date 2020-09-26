using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Plans.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Plans.Command.CreatePlan
{
    public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, Result<PlanDto>>
    {
        private readonly IMongoRepository<Plan> _planRepository;
        private readonly IMapper _mapper;

        public CreatePlanCommandHandler(IMongoRepository<Plan> planRepository, IMapper mapper)
        {
            _planRepository = planRepository;
            _mapper = mapper;
        }

        public async Task<Result<PlanDto>> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = _mapper.Map<Plan>(request);

            await _planRepository.AddAsync(plan);

            return Result<PlanDto>.SuccessFul(_mapper.Map<PlanDto>(plan));
        }
    }
}