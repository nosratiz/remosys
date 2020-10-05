using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Tickets.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Tickets.Queries
{
    public class GetTicketQuery : IRequest<Result<TicketDto>>
    {
        public Guid Id { get; set; }

    }


    public class GetTicketQueryHandler : IRequestHandler<GetTicketQuery, Result<TicketDto>>
    {
        private readonly IMongoRepository<Ticket> _ticketServices;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetTicketQueryHandler(IMongoRepository<Ticket> ticketServices, IMapper mapper, ICurrentUserService currentUserService)
        {
            _ticketServices = ticketServices;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<TicketDto>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketServices.GetAsync(x => x.IsDeleted == false && x.Id == request.Id, cancellationToken);

            if (ticket is null)
                return Result<TicketDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.TicketNotFound)));

            if (_currentUserService.RoleName != Role.Admin && ticket.User.Id != Guid.Parse(_currentUserService.UserId))
                return Result<TicketDto>.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.ContentNotAccess)));
            

            return Result<TicketDto>.SuccessFul(_mapper.Map<TicketDto>(ticket));
        }
    }
}