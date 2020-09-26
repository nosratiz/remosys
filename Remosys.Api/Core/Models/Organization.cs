using System;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Organization : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual User Manager { get; set; }
    }
}