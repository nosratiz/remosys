using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Organization.Dto;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Organization.Queries
{
    public class GetOrganizationQuery : IRequest<Result<OrganizationDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, Result<OrganizationDto>>
    {
        private readonly IMongoRepository<Models.Organization> _organizationRepository;

        private readonly IMapper _mapper;

        public GetOrganizationQueryHandler(IMongoRepository<Models.Organization> organizationRepository, IMapper mapper)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrganizationDto>> Handle(GetOrganizationQuery request,
            CancellationToken cancellationToken)
        {
            var organization =
                await _organizationRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id,
                    cancellationToken);

            if (organization is null)
                return Result<OrganizationDto>.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.OrganizationNotFound)));

            return Result<OrganizationDto>.SuccessFul(_mapper.Map<OrganizationDto>(organization));
        }
    }
}