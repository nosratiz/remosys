using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.ToolCategories.Command.Create;
using Remosys.Api.Core.Application.ToolCategories.Command.Update;
using Remosys.Api.Core.Application.ToolCategories.Queries;
using Remosys.Api.Core.Application.Tools.Command.Delete;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ToolCategoriesController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new GetToolCategoryPagedListQuery
            {
                Page = pagingOptions.Page,
                Limit = pagingOptions.Limit,
                Query = pagingOptions.Query
            }));


        [HttpGet("{id}", Name = "GetToolCategoryInfo")]
        public async Task<IActionResult> GetToolCategory(Guid id)
        {
            var result = await Mediator.Send(new GetCategoryQuery { Id = id });

            return result.ApiResult;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteToolCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommand createCategoryCommand)
        {
            var result = await Mediator.Send(createCategoryCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetToolCategoryInfo", new { id = result.Data.Id }), result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryCommand updateCategoryCommand)
        {
            var result = await Mediator.Send(updateCategoryCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
