using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Remosys.Common.Helper.systemMessage;
using Remosys.Common.Mongo;
using Remosys.Common.Result;

namespace Remosys.Api.Core.Application.AgentSetting.Command.Delete
{
    public class DeleteAgentSettingCommandHandler : IRequestHandler<DeleteAgentSettingCommand,Result>
    {
        private readonly IMongoRepository<Models.AgentSetting> _agentSettingRepository;

        public DeleteAgentSettingCommandHandler(IMongoRepository<Models.AgentSetting> agentSettingRepository)
        {
            _agentSettingRepository = agentSettingRepository;
        }

        public async Task<Result> Handle(DeleteAgentSettingCommand request, CancellationToken cancellationToken)
        {
            var agentSetting = await _agentSettingRepository.GetAsync(request.Id, cancellationToken);

            if (agentSetting is null)
                return Result.Failed(
                    new NotFoundObjectResult(new ApiMessage(ResponseMessage.SettingNotFound)));

            agentSetting.IsDeleted = true;

            await _agentSettingRepository.UpdateAsync(agentSetting);

            return Result.SuccessFul();
        }
    }
}