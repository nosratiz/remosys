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

namespace Remosys.Api.Core.Application.Organization.Command.Update
{
    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, Result>
    {
        private readonly IMongoRepository<Models.Organization> _organizationRepository;
        private readonly IMongoRepository<Employee> _employeeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateOrganizationCommandHandler(IMongoRepository<Models.Organization> organizationRepository,
            IMongoRepository<Employee> employeeRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var organization =
                await _organizationRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.Id,
                    cancellationToken);

            if (organization is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.OrganizationNotFound)));

            if (organization.Manager.Id != Guid.Parse(_currentUserService.UserId) && _currentUserService.RoleName == Role.Manager)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(ResponseMessage.JustCreatorAllowed)));


            var employee = await _employeeRepository
                .GetAsync(x => x.IsDeleted == false && x.Id == request.EmployeeId,
                    cancellationToken);

            if (employee is null)
                return Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(ResponseMessage.EmployeeNotFound)));

            #endregion

            _mapper.Map(request, organization);

            await _organizationRepository.UpdateAsync(organization);

            return Result.SuccessFul();
        }
    }
}