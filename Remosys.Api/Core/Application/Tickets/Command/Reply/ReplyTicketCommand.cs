using System;
using MediatR;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tickets.Command.Reply
{
    public class ReplyTicketCommand : IRequest<Result>
    {
        public Guid ParentId { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
    }
}