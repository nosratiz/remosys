using System;
using Remosys.Api.Core.Application.Plans.Dto;

namespace Remosys.Api.Core.Application.Contracts.Dto
{
    public class ContractDto
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartContract { get; set; }
        public DateTime EndContract { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public virtual PlanDto Plan { get; set; }


    }
}