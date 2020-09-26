using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Contracts.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Contracts.Queries
{
    public class GetPagedListContractQuery : PagingOptions, IRequest<PagedResult<ContractDto>>
    {

    }
    public class GetPagedListContractQueryHandler : IRequestHandler<GetPagedListContractQuery, PagedResult<ContractDto>>
    {
        private readonly IMongoRepository<Contract> _contractRepository;
        private readonly IMapper _mapper;

        public GetPagedListContractQueryHandler(IMongoRepository<Contract> contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ContractDto>> Handle(GetPagedListContractQuery request, CancellationToken cancellationToken)
        {
            var contract = _contractRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                contract = contract.Where(x => x.User.FirstName.Contains(request.Query)
                                               || x.User.LastName.Contains(request.Query)
                                               || x.User.Email.Contains(request.Query)
                                               || x.User.Mobile == request.Query);
            }

            var contractList = await _contractRepository.BrowseAsync(contract, request);

            return _mapper.Map<PagedResult<ContractDto>>(contractList);
        }
    }
}