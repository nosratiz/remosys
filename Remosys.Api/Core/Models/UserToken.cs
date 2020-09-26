using System;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class UserToken : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string Ip { get; set; }
        public string Os { get; set; }
        public string Device { get; set; }
        public string UserAgent { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsExpired { get; set; }
    }
}