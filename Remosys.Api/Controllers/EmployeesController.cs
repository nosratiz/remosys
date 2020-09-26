using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Departments.Command.Delete;
using Remosys.Api.Core.Application.Employees.Command.Create;
using Remosys.Api.Core.Application.Employees.Command.Update;
using Remosys.Api.Core.Application.Employees.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class EmployeesController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new GetEmployeePagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Query = pagingOptions.Query
            }));



        [HttpGet("{id}", Name = "GetEmployeeInfo")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetEmployeeQuery { Id = id });

            return result.ApiResult;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteDepartmentCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeCommand createEmployee)
        {
            var result = await Mediator.Send(createEmployee);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetEmployeeInfo", new { id = result.Data.Id }), result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateEmployeeCommand updateEmployeeCommand)
        {
            var result = await Mediator.Send(updateEmployeeCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
