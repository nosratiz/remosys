using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Contracts.Command.CreateContract;
using Remosys.Api.Core.Application.Contracts.Command.DeleteContract;
using Remosys.Api.Core.Application.Contracts.Command.UpdateContract;
using Remosys.Api.Core.Application.Contracts.Queries;
using Remosys.Common.Helper.Pagination;

namespace Remosys.Api.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class ContractsController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetContract([FromQuery] PagingOptions pagingOptions)
        =>
          Ok(await Mediator.Send(new GetPagedListContractQuery
          {
              Limit = pagingOptions.Limit,
              Page = pagingOptions.Page,
              Query = pagingOptions.Query
          }));


        [HttpGet("MyContract")]
        public async Task<IActionResult> GetMyContract([FromQuery] PagingOptions pagingOptions)
            => Ok(await Mediator.Send(new MyContractQuery
            {
                Limit = pagingOptions.Limit,
                Page = pagingOptions.Page,
                Query = pagingOptions.Query
            }));



        [HttpGet("{id}", Name = "GetContractInfo")]
        public async Task<IActionResult> GetContract(Guid id)
        {
            var result = await Mediator.Send(new GetContractQuery { Id = id });

            return result.ApiResult;
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteContractCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateContractCommand createContractCommand)
        {
            var result = await Mediator.Send(createContractCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetContractInfo", new { id = result.Data.Id }), result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(UpdateContractCommand updateContractCommand)
        {
            var result = await Mediator.Send(updateContractCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }
    }
}
