using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Remosys.Common.Helper.Claims
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("Id");
            IsAuthenticated = UserId != null;
            FullName = httpContextAccessor.HttpContext?.User?.FindFirstValue("fullName");
            RoleName = httpContextAccessor.HttpContext?.User?.FindFirstValue("RoleName");
        }

        public string UserId { get; }
        public string FullName { get; }
        public string RoleName { get; set; }
        public bool IsAuthenticated { get; }
    }
}