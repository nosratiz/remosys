using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tools.Command.Delete
{
    public class DeleteToolCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}