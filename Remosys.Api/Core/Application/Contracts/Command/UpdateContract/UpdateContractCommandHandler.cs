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

namespace Remosys.Api.Core.Application.Contracts.Command.UpdateContract
{
    public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, Result>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<Plan> _planRepository;
        private readonly IMongoRepository<Contract> _contractRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateContractCommandHandler(IMongoRepository<User> userRepository, IMongoRepository<Plan> planRepository, IMongoRepository<Contract> contractRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _planRepository = planRepository;
            _contractRepository = contractRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var contract = await _contractRepository.GetAsync(request.Id, cancellationToken);

            if (contract is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.ContractNotFound)));

            var user = await _userRepository.GetAsync(x => x.IsDelete == false && x.Id == Guid.Parse(_currentUserService.UserId), cancellationToken);

            if (user is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            var plan = await _planRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.PlanId, cancellationToken);

            if (plan is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.PlanNotFound)));


            #endregion

            _mapper.Map(request, contract);

            contract.User = user;
            contract.Plan = plan;

            await _contractRepository.UpdateAsync(contract);

            return Result.SuccessFul();
        }
    }
}