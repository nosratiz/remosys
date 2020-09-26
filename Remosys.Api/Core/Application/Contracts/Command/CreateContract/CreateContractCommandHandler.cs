using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Contracts.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.Claims;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Contracts.Command.CreateContract
{
    public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, Result<ContractDto>>
    {
        private readonly IMongoRepository<User> _userRepository;
        private readonly IMongoRepository<Plan> _planRepository;
        private readonly IMongoRepository<Contract> _contractRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateContractCommandHandler(IMongoRepository<User> userRepository, IMapper mapper, IMongoRepository<Plan> planRepository, IMongoRepository<Models.Contract> contractRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _planRepository = planRepository;
            _contractRepository = contractRepository;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ContractDto>> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var user = await _userRepository.GetAsync(x => x.IsDelete == false && x.Id == Guid.Parse(_currentUserService.UserId), cancellationToken);

            if (user is null)
                return Result<ContractDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.UserNotFound)));

            var plan = await _planRepository.GetAsync(x => x.IsDeleted == false && x.Id == request.PlanId, cancellationToken);

            if (plan is null)
                return Result<ContractDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.PlanNotFound)));


            #endregion

            var contract = _mapper.Map<Contract>(request);

            contract.EndContract = DateTime.Now.AddMonths(plan.Month);

            contract.User = user;
            contract.Plan = plan;

            await _contractRepository.AddAsync(contract);

            return Result<ContractDto>.SuccessFul(_mapper.Map<ContractDto>(contract));
        }
    }
}