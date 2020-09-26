using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Departments.Command.Create;
using Remosys.Api.Core.Application.Departments.Command.Delete;
using Remosys.Api.Core.Application.Departments.Command.Update;
using Remosys.Api.Core.Application.Departments.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{

    [Authorize(Roles = "Admin,Manager")]
    public class DepartmentsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new GetDepartmentPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Query = pagingOptions.Query
            }));


        [HttpGet("{id}", Name = "GetDepartmentInfo")]
        public async Task<IActionResult> GetDepartment(Guid id)
        {
            var result = await Mediator.Send(new GetDepartmentQuery { Id = id });

            return result.ApiResult;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
            var result = await Mediator.Send(new DeleteDepartmentCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentCommand createDepartment)
        {
            var result = await Mediator.Send(createDepartment);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetDepartmentInfo", new { id = result.Data.Id }), result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentCommand updateDepartment)
        {
            var result = await Mediator.Send(updateDepartment);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

    }
}
