using System;
using System.Collections.Generic;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Permission : IIdentifiable
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}