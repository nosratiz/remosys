using System;
using MediatR;
using Remosys.Api.Core.Application.Contracts.Dto;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Contracts.Command.CreateContract
{
    public class CreateContractCommand : IRequest<Result<ContractDto>>
    {
        public Guid PlanId { get; set; }
    }
}