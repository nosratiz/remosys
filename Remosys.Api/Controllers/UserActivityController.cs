using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Activity.Command;

namespace Remosys.Api.Controllers
{
    [Authorize]
    public class UserActivityController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserActivityCommand createUserActivity)
        {
            await Mediator.Send(createUserActivity);

            return NoContent();
        }



    }
}
