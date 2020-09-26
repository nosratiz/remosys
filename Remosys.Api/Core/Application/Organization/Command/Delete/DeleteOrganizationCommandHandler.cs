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

namespace Remosys.Api.Core.Application.Organization.Command.Delete
{
    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, Result>
    {
        private readonly IMongoRepository<Models.Organization> _organizationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public DeleteOrganizationCommandHandler(IMongoRepository<Models.Organization> organizationRepository,
            IMapper mapper, ICurrentUserService currentUserService)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            var organization =
                await _organizationRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id,
                    cancellationToken);

            if (organization is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.OrganizationNotFound)));

            if (organization.Manager.Id != Guid.Parse(_currentUserService.UserId) && _currentUserService.RoleName == Role.Manager)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.DeleteNotAllowed)));


            organization.IsDeleted = true;

            await _organizationRepository.UpdateAsync(organization);

            return Result.SuccessFul();
        }
    }
}