using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Api.Core.Models;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.Contracts.Command.DeleteContract
{
    public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand, Result>
    {
        private readonly IMongoRepository<Contract> _contractRepository;

        public DeleteContractCommandHandler(IMongoRepository<Contract> contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<Result> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            var contract = await _contractRepository.GetAsync(request.Id, cancellationToken);

            if (contract is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(ResponseMessage.ContractNotFound)));
            


            await _contractRepository.DeleteAsync(request.Id);


            return Result.SuccessFul();
        }
    }
}