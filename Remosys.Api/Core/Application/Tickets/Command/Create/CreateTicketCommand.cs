using System;
using MediatR;
using Remosys.Api.Core.Application.Tickets.Dto;
using Remosys.Common.Enums;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tickets.Command.Create
{
    public class CreateTicketCommand : IRequest<Result<TicketDto>>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
    }
}