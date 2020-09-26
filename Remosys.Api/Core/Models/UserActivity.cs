using System;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class UserActivity : IIdentifiable
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string ApplicationName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
