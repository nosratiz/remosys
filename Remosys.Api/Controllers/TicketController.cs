using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Tickets.Command.Create;
using Remosys.Api.Core.Application.Tickets.Command.Delete;
using Remosys.Api.Core.Application.Tickets.Command.Reply;
using Remosys.Api.Core.Application.Tickets.Queries;

namespace Remosys.Api.Controllers
{
    [Authorize]
    public class TicketController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetTicketPagedListQuery getTicketPagedList) =>
            Ok(await Mediator.Send(getTicketPagedList));



        [HttpGet("{id}", Name = "GetTicketInfo")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetTicketQuery { Id = id });

            return result.ApiResult;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteTicketCommand { Id = id });

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketCommand createTicketCommand)
        {
            var result = await Mediator.Send(createTicketCommand);

            if (result.Success == false)
                return result.ApiResult;

            return Created(Url.Link("GetTicketInfo", new { id = result.Data.Id }), result.Data);
        }


        [HttpPut]
        public async Task<IActionResult> Update(ReplyTicketCommand replyTicketCommand)
        {
            var result = await Mediator.Send(replyTicketCommand);

            if (result.Success == false)
                return result.ApiResult;

            return NoContent();
        }

    }
}
