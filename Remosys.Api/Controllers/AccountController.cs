using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Auth.Command.ConfirmCode;
using Remosys.Api.Core.Application.Auth.Command.LoginCommand;
using Remosys.Api.Core.Application.Auth.Command.RegisterCommand;
using Remosys.Api.Core.Application.Auth.Command.RegisterEmployeeCommand;
using Remosys.Api.Core.Application.Auth.Command.SendConfirmCode;
using Remosys.Api.Core.Application.Users.Command.UpdateProfile;
using Remosys.Api.Core.Application.Users.Queries;

namespace Remosys.Api.Controllers
{
    public class AccountController : BaseController
    {


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand loginCommand)
        {
            var result = await Mediator.Send(loginCommand);

            return result.ApiResult;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCommand registerCommand)
        {
            var result = await Mediator.Send(registerCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Ok(result.Data);
        }


        [HttpPost("ConfirmCode")]
        public async Task<IActionResult> ConfirmCode(ConfirmCodeCommand confirmCode)
        {
            var result = await Mediator.Send(confirmCode);

            if (result.Success == false)
                return result.ApiResult;

            return Ok(result.Data);
        }

        [HttpPost("SendConfirmCode")]
        public async Task<IActionResult> SendConfirmCode(SendConfirmCodeCommand sendConfirmCodeCommand)
        {
            var result = await Mediator.Send(sendConfirmCodeCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeCommand registerEmployee)
        {
            var result = await Mediator.Send(registerEmployee);

            if (result.Success == false)
                return result.ApiResult;

            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await Mediator.Send(new GetAccountInfoQuery());

            return result.ApiResult;
        }

        [Authorize]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand updateProfileCommand)
        {
            var result = await Mediator.Send(updateProfileCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
