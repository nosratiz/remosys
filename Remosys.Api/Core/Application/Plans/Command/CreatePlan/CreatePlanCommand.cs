using MediatR;
using Remosys.Api.Core.Application.Plans.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Plans.Command.CreatePlan
{
    public class CreatePlanCommand : IRequest<Result<PlanDto>>
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int PersonCount { get; set; }

        public int Month { get; set; }
    }
}