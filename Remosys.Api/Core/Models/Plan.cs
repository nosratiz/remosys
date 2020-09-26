using System;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class Plan : IIdentifiable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int PersonCount { get; set; }

        public int Month { get; set; }

        public bool IsDeleted { get; set; }
    }
}