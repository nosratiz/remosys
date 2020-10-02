using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Users.Command.CreateUser;
using Remosys.Api.Core.Application.Users.Command.DeleteUser;
using Remosys.Api.Core.Application.Users.Command.UpdateUser;
using Remosys.Api.Core.Application.Users.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetUserList([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new GetUserPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Query = pagingOptions.Query
            }));


        [HttpGet("{id}", Name = "UserInfo")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetUserQuery { Id = id });

            return result.ApiResult;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand createUserCommand)
        {
            var result = await Mediator.Send(createUserCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("UserInfo", new { id = result.Data.Id }), result.Data);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteUserCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();

        }


        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
        {
            var result = await Mediator.Send(updateUserCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }



    }
}
