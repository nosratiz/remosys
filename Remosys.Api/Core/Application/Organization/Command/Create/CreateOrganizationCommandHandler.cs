using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Organization.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Organization.Command.Create
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Result<OrganizationDto>>
    {
        private readonly IMongoRepository<Models.Organization> _organizationRepository;
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<Contract> _contractRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrganizationCommandHandler(IMongoRepository<Models.Organization> organizationRepository,
            IMapper mapper, IMongoRepository<User> userRepository, ICurrentUserService currentUserService, IMongoRepository<Contract> contractRepository)
        {
            _organizationRepository = organizationRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _contractRepository = contractRepository;
        }

        public async Task<Result<OrganizationDto>> Handle(CreateOrganizationCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository
                .GetAsync(x => x.IsDelete == false && x.Id == Guid.Parse(_currentUserService.UserId),
                    cancellationToken);

            if (user is null)
                return Result<OrganizationDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

            var userContract = await _contractRepository.GetAsync(x => x.User.Id == Guid.Parse(_currentUserService.UserId), cancellationToken);

            if (userContract is null)
                return Result<OrganizationDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.UserHasNoContract)));

            if (user.Organizations != null && _currentUserService.RoleName != Role.Admin)
                return Result<OrganizationDto>.Failed(
                    new BadRequestObjectResult(ResponseMessage.MaxOrganizationReached));


            var organization = _mapper.Map<Models.Organization>(request);

            organization.Manager = user;

            await _organizationRepository.AddAsync(organization);

            //cause cycle in Mongo db 
            organization.Manager = null;
            user.Organizations = new List<Models.Organization> { organization };

            await _userRepository.UpdateAsync(user);

            return Result<OrganizationDto>.SuccessFul(_mapper.Map<OrganizationDto>(organization));
        }
    }
}