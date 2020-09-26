using System;

namespace Remosys.Api.Core.Application.Plans.Dto
{
    public class PlanDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int PersonCount { get; set; }

        public int Month { get; set; }
    }
}