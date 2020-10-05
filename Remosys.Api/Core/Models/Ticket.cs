using System;
using System.Collections.Generic;
using Remosys.Common.Enums;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Ticket : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Ticket> Children { get; set; }
    }
}