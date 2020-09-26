using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Contracts.Command.UpdateContract
{
    public class UpdateContractCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid PlanId { get; set; }
    }
}