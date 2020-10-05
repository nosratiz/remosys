using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tickets.Command.Delete
{
    public class DeleteTicketCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}