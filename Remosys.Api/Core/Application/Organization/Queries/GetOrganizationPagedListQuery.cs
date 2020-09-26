using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Organization.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Mongo;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Application.Organization.Queries
{
    public class GetOrganizationPagedListQuery : PagingOptions, IRequest<PagedResult<OrganizationDto>>
    {
    }


    public class GetOrganizationPagedListQueryHandler : IRequestHandler<GetOrganizationPagedListQuery,
            PagedResult<OrganizationDto>>
    {
        private readonly IMongoRepository<Models.Organization> _organizationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetOrganizationPagedListQueryHandler(IMongoRepository<Models.Organization> organizationRepository,
            IMapper mapper, ICurrentUserService currentUserService)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<OrganizationDto>> Handle(GetOrganizationPagedListQuery request,
            CancellationToken cancellationToken)
        {
            var organization = _organizationRepository.GetAll().Where(x => x.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(request.Query))
                organization = organization.Where(x => x.Name.Contains(request.Query)
                                                       || x.Address.Contains(request.Query));

            if (_currentUserService.RoleName == Role.Manager)
                organization = organization.Where(x => x.Manager.Id == Guid.Parse(_currentUserService.UserId));
            

            var organizationList = await _organizationRepository.BrowseAsync(organization, request);

            return _mapper.Map<PagedResult<OrganizationDto>>(organizationList);
        }
    }
}