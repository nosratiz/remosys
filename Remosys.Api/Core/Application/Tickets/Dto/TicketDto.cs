﻿using System;
using System.Collections.Generic;
using Remosys.Api.Core.Application.Users.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Enums;

namespace Remosys.Api.Core.Application.Tickets.Dto
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public virtual UserDto User { get; set; }
    }
}