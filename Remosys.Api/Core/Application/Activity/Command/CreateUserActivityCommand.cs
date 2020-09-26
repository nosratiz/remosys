using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Remosys.Api.Core.Application.Activity.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Activity.Command
{
    public class CreateUserActivityCommand : IRequest<Result>
    {
        public List<CreateUserActivityDto> UserActivities { get; set; }
    }

    public class CreateUserActivityCommandHandler : IRequestHandler<CreateUserActivityCommand, Result>
    {
        private readonly IMongoRepository<UserActivity> _userActivityRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateUserActivityCommandHandler(IMongoRepository<UserActivity> userActivityRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _userActivityRepository = userActivityRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(CreateUserActivityCommand request, CancellationToken cancellationToken)
        {
            var userActivities = _mapper.Map<List<UserActivity>>(request.UserActivities);

            userActivities.ForEach(x =>
            {
                x.UserId = Guid.Parse(_currentUserService.UserId);
                x.FullName = _currentUserService.FullName;
            });

            await _userActivityRepository.AddRangeAsync(userActivities);

            return Result.SuccessFul();
        }
    }
}