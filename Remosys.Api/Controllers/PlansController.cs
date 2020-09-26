using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Plans.Command.CreatePlan;
using Remosys.Api.Core.Application.Plans.Command.DeletePlan;
using Remosys.Api.Core.Application.Plans.Command.UpdatePlan;
using Remosys.Api.Core.Application.Plans.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlansController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPlanList([FromQuery] PagingOptions paging)
            => Ok(await Mediator.Send(new GetPlanPagedListQuery
            {
                Limit = paging.Limit,
                Page = paging.Page,
                Query = paging.Query
            }));

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetPlanInfo")]
        public async Task<IActionResult> GetPlan(Guid id)
        {
            var result = await Mediator.Send(new GetPlanQuery { Id = id });

            return result.ApiResult;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeletePlanCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        
        [HttpPost]
        public async Task<IActionResult> CreatePlan(CreatePlanCommand createPlanCommand)
        {
            var result = await Mediator.Send(createPlanCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetPlanInfo", new { id = result.Data.Id }), result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> UpdatePlan(UpdatePlanCommand updatePlanCommand)
        {
            var result = await Mediator.Send(updatePlanCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
