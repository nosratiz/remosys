using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Plans.Command.UpdatePlan
{
    public class UpdatePlanCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }

        public int PersonCount { get; set; }

        public int Month { get; set; }
    }
}