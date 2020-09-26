using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Tools.Command.Create;
using Remosys.Api.Core.Application.Tools.Command.Delete;
using Remosys.Api.Core.Application.Tools.Command.Update;
using Remosys.Api.Core.Application.Tools.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ToolsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new GetToolPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Query = pagingOptions.Query
            }));


        [HttpGet("{id}", Name = "GetToolInfo")]
        public async Task<IActionResult> GetTool(Guid id)
        {
            var result = await Mediator.Send(new GetToolQuery { Id = id });

            return result.ApiResult;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTool(Guid id)
        {
            var result = await Mediator.Send(new DeleteToolCommand { Id = id });

            return result.ApiResult;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateToolCommand createToolCommand)
        {
            var result = await Mediator.Send(createToolCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetToolInfo", new { id = result.Data.Id }), result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateToolCommand updateToolCommand)
        {
            var result = await Mediator.Send(updateToolCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
