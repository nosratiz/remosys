using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Application.Contracts.Dto;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Contracts.Queries
{
    public class GetContractQuery : IRequest<Result<ContractDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetContractQueryHandler : IRequestHandler<GetContractQuery, Result<ContractDto>>
    {
        private readonly IMongoRepository<Contract> _contractRepository;
        private readonly IMapper _mapper;

        public GetContractQueryHandler(IMongoRepository<Contract> contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        public async Task<Result<ContractDto>> Handle(GetContractQuery request, CancellationToken cancellationToken)
        {
            var contract = await _contractRepository.GetAsync(request.Id, cancellationToken);

            if (contract is null)
                return Result<ContractDto>.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.ContractNotFound)));


            return Result<ContractDto>.SuccessFul(_mapper.Map<ContractDto>(contract));
        }
    }
}