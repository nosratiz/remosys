using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Tickets.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Enums;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Tickets.Queries
{
    public class GetTicketPagedListQuery : PagingOptions, IRequest<PagedResult<TicketListDto>>
    {
        public TicketStatus? TicketStatus { get; set; }
    }

    public class GetTicketPagedListQueryHandler : IRequestHandler<GetTicketPagedListQuery, PagedResult<TicketListDto>>
    {
        private readonly IMongoRepository<Ticket> _ticketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;


        public GetTicketPagedListQueryHandler(IMongoRepository<Ticket> ticketRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<TicketListDto>> Handle(GetTicketPagedListQuery request, CancellationToken cancellationToken)
        {
            var ticket = _ticketRepository.GetAll().Where(x => x.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(request.Query))
                ticket = ticket.Where(x => x.Title.Contains(request.Query) ||
                                           x.User.LastName.Contains(request.Query) ||
                                           x.User.FirstName.Contains(request.Query) ||
                                           x.User.Mobile.Contains(request.Query));

            if (request.TicketStatus.HasValue)
                ticket = ticket.Where(x => x.TicketStatus == request.TicketStatus.Value);

            if (_currentUserService.RoleName != Role.Admin)
                ticket = ticket.Where(x => x.User.Id == Guid.Parse(_currentUserService.UserId));
            
            var ticketList = await _ticketRepository.BrowseAsync(ticket, request);

            return _mapper.Map<PagedResult<TicketListDto>>(ticketList);

        }
    }
}