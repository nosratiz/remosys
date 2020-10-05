using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;
using Guid = System.Guid;

namespace Remosys.Api.Core.Application.Tickets.Command.Delete
{
    public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, Result>
    {
        private readonly IMongoRepository<Ticket> _ticketRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTicketCommandHandler(IMongoRepository<Ticket> ticketRepository, ICurrentUserService currentUserService)
        {
            _ticketRepository = ticketRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket =
                await _ticketRepository.GetAsync(x => x.IsDeleted == false && (x.Id == request.Id || x.Children.Any(x => x.Id == request.Id)), cancellationToken);

            if (ticket is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.TicketNotFound)));

            if (_currentUserService.RoleName != Role.Admin && ticket.User.Id != Guid.Parse(_currentUserService.UserId))
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.DeleteNotAllowed)));


            ticket.IsDeleted = true;

            await _ticketRepository.UpdateAsync(ticket);

            return Result.SuccessFul();

        }
    }
}