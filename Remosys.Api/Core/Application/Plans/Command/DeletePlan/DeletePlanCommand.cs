using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Plans.Command.DeletePlan
{
    public class DeletePlanCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}