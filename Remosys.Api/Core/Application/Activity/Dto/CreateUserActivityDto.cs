using System;

namespace Remosys.Api.Core.Application.Activity.Dto
{
    public class CreateUserActivityDto
    {
        public string ApplicationName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}