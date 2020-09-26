using System;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Employee : IIdentifiable
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public string Position { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartContract { get; set; }
        public DateTime? EndContract { get; set; }


        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public virtual User User { get; set; }
    }
}