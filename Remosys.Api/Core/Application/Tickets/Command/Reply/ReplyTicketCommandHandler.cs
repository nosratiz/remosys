using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tickets.Command.Reply
{
    public class ReplyTicketCommandHandler : IRequestHandler<ReplyTicketCommand, Result>
    {
        private readonly IMongoRepository<Ticket> _ticketRepository;
        private readonly IMongoRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public ReplyTicketCommandHandler(IMongoRepository<Ticket> ticketRepository, ICurrentUserService currentUser, IMapper mapper, IMongoRepository<User> userRepository)
        {
            _ticketRepository = ticketRepository;
            _currentUser = currentUser;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(ReplyTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket =
                await _ticketRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.ParentId, cancellationToken);

            if (ticket is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.TicketNotFound)));

            var user = await _userRepository.GetAsync(x => x.Id == Guid.Parse(_currentUser.UserId), cancellationToken);

            var replyTicket = _mapper.Map<Ticket>(request);

            replyTicket.User = user;

            ticket.Children.Add(replyTicket);

            await _ticketRepository.UpdateAsync(ticket);

            return Result.SuccessFul();
        }
    }
}