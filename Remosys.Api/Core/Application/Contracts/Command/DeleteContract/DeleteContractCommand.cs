using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Contracts.Command.DeleteContract
{
    public class DeleteContractCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}