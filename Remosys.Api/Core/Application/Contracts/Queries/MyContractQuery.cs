using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Contracts.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Result;
using Remosys.Common.Types;


namespace Remosys.Api.Core.Application.Contracts.Queries
{
    public class MyContractQuery : PagingOptions, IRequest<PagedResult<ContractDto>>
    {

    }


    public class MyContractQueryHandler : IRequestHandler<MyContractQuery, PagedResult<ContractDto>>
    {
        private readonly IMongoRepository<Contract> _contractRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public MyContractQueryHandler(IMongoRepository<Contract> contractRepository, IMapper mapper, ICurrentUserService currentUser)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<ContractDto>> Handle(MyContractQuery request, CancellationToken cancellationToken)
        {
            var contract = await _contractRepository.BrowseAsync(x => x.User.Id == Guid.Parse(_currentUser.UserId), request);

            return _mapper.Map<PagedResult<ContractDto>>(contract);
        }
    }
}