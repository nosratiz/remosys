using System;

namespace Remosys.Api.Core.Application.Activity.Dto
{
    public class UserActivityDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string ApplicationName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}