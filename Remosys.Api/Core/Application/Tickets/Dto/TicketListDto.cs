using System;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Common.Enums;

namespace Remosys.Api.Core.Application.Tickets.Dto
{
    public class TicketListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual UserDto User { get; set; }
        public TicketStatus TicketStatus { get; set; }
    }
}