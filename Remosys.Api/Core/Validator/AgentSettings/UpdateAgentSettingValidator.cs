using FluentValidation;
using Remosys.Api.Core.Application.AgentSetting.Command.Update;
using Remosys.Common.Helper.systemMessage;

namespace Remosys.Api.Core.Validator.AgentSettings
{
    public class UpdateAgentSettingValidator : AbstractValidator<UpdateAgentSettingCommand>
    {
        public UpdateAgentSettingValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Id).NotEmpty()
                .WithMessage(ResponseMessage.IdIsRequired).NotNull().WithMessage(ResponseMessage.IdIsRequired);

            RuleFor(dto => dto.Name).NotEmpty()
                .WithMessage(ResponseMessage.NameIsRequired)
                .NotNull().WithMessage(ResponseMessage.NameIsRequired);


            RuleFor(dto => dto.ScreenShotSetting).NotEmpty().NotNull();

            RuleFor(dto => dto.ScreenShotSetting.ScreenShotPerHour)
                .NotEmpty().NotNull().GreaterThanOrEqualTo(0);


            RuleFor(dto => dto.TimeTracking).NotEmpty().NotNull();

            RuleFor(dto => dto.TimeSetting).NotEmpty().NotNull();

            RuleFor(dto => dto.UserAccess).NotEmpty().NotNull();

            RuleFor(dto => dto.UserInterAction).NotEmpty().NotNull();

            RuleFor(dto => dto.Task).NotEmpty().NotNull();
        }
    }
}