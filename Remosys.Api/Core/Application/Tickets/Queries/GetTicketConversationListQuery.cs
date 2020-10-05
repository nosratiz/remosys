using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Tickets.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tickets.Queries
{
    public class GetTicketConversationListQuery : IRequest<Result<List<TicketDto>>>
    {
        public Guid Id { get; set; }
    }

    public class GetTicketConversationListQueryHandler : IRequestHandler<GetTicketConversationListQuery, Result<List<TicketDto>>>
    {
        private readonly IMongoRepository<Ticket> _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketConversationListQueryHandler(IMongoRepository<Ticket> ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<TicketDto>>> Handle(GetTicketConversationListQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id, cancellationToken);

            if (ticket is null)
                return Result<List<TicketDto>>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.TicketNotFound)));


            var ticketList = _mapper.Map<List<TicketDto>>(ticket.Children);



            return Result<List<TicketDto>>.SuccessFul(ticketList);
        }
    }
}