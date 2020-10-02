using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.AgentSetting.Command.Create;
using Remosys.Api.Core.Application.AgentSetting.Command.Delete;
using Remosys.Api.Core.Application.AgentSetting.Command.Update;
using Remosys.Api.Core.Application.AgentSetting.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    public class AgentController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new GetAgentSettingPagedListQuery
            {
                Limit = pagingOptions.Limit,
                Page = pagingOptions.Page,
                Query = pagingOptions.Query
            }));


        [HttpGet("{id}", Name = "GetAgentSettingInfo")]
        public async Task<IActionResult> GetAgentSetting(Guid id)
        {
            var result = await Mediator.Send(new GetAgentSettingQuery { Id = id });

            return result.ApiResult;
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgentSetting(Guid id)
        {
            var result = await Mediator.Send(new DeleteAgentSettingCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAgentSetting(CreateAgentSettingCommand createAgentSetting)
        {
            var result = await Mediator.Send(createAgentSetting);

            if (result.Success == false)
                return result.ApiResult;

            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateAgentSetting(UpdateAgentSettingCommand updateAgentSetting)
        {
            var result = await Mediator.Send(updateAgentSetting);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
