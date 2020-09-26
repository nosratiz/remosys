using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Departments.Command.Delete;
using Remosys.Api.Core.Application.Organization.Command.Create;
using Remosys.Api.Core.Application.Organization.Command.Update;
using Remosys.Api.Core.Application.Organization.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class OrganizationsController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new GetOrganizationPagedListQuery
            {
                Page = pagingOptions.Page,
                Query = pagingOptions.Query,
                Limit = pagingOptions.Limit
            }));


        [HttpGet("{id}", Name = "GetOrganizationInfo")]
        public async Task<IActionResult> GetOrganization(Guid id)
        {
            var result = await Mediator.Send(new GetOrganizationQuery { Id = id });

            return result.ApiResult;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(Guid id)
        {
            var result = await Mediator.Send(new DeleteDepartmentCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateOrganizationCommand createOrganizationCommand)
        {
            var result = await Mediator.Send(createOrganizationCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetOrganizationInfo", new { id = result.Data.Id }), result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateOrganizationCommand updateOrganizationCommand)
        {
            var result = await Mediator.Send(updateOrganizationCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
