using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Plans.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Plans.Queries
{
    public class GetPlanQuery : IRequest<Result<PlanDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetPlanQueryHandler : IRequestHandler<GetPlanQuery, Result<PlanDto>>
    {
        private readonly IMongoRepository<Plan> _planRepository;
        private readonly IMapper _mapper;

        public GetPlanQueryHandler(IMongoRepository<Plan> planRepository, IMapper mapper)
        {
            _planRepository = planRepository;
            _mapper = mapper;
        }

        public async Task<Result<PlanDto>> Handle(GetPlanQuery request, CancellationToken cancellationToken)
        {
            var plan = await _planRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id, cancellationToken);

            if (plan is null)
                return Result<PlanDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.PlanNotFound)));

            return Result<PlanDto>.SuccessFul(_mapper.Map<PlanDto>(plan));
        }
    }
}