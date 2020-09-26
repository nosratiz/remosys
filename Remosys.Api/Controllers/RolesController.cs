using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Roles.Queries;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetRoleList()
        {
            var roles = await Mediator.Send(new GetRoleListQuery());

            return Ok(roles);
        }
    }
}
