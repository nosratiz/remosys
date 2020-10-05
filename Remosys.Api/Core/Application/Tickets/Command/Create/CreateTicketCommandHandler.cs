using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Tickets.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tickets.Command.Create
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Result<TicketDto>>
    {
        private readonly IMongoRepository<Ticket> _ticketRepository;
        private readonly IMongoRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;


        public CreateTicketCommandHandler(IMongoRepository<Ticket> ticketRepository, IMapper mapper, IMongoRepository<User> userRepository, ICurrentUserService currentUserService)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result<TicketDto>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = _mapper.Map<Ticket>(request);

            var user = await _userRepository.GetAsync(x =>
                 x.IsDelete == false && x.Id == Guid.Parse(_currentUserService.UserId), cancellationToken);

            ticket.User = user;

            await _ticketRepository.AddAsync(ticket);

            return Result<TicketDto>.SuccessFul(_mapper.Map<TicketDto>(ticket));

        }
    }
}